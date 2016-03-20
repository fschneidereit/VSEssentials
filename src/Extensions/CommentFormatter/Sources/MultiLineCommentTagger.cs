/***************************************************************************************************
 *
 *  Copyright © 2015-2016 Florian Schneidereit
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 *  and associated documentation files (the "Software"), to deal in the Software without
 *  restriction, including without limitation the rights to use, copy, modify, merge, publish,
 *  distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 *  Software is furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all copies or
 *  substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 *  BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 *  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 *  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 **************************************************************************************************/

#region Using Directives

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using CS = Microsoft.CodeAnalysis.CSharp;

#endregion

namespace VSEssentials.CommentFormatter
{
    internal class MultiLineCommentTagger : ITagger<IClassificationTag>
    {
        #region Fields

        private MultiLineCommentTaggerContext _context;
        private readonly IClassificationType _blockCommentType;
        private readonly VisualStudioWorkspace _workspace;

        #endregion

        #region Constructors

        public MultiLineCommentTagger(VisualStudioWorkspace workspace, IClassificationTypeRegistryService classificationTypeRegistry)
        {
            // Instance initialization
            _context = MultiLineCommentTaggerContext.Empty;
            _blockCommentType = classificationTypeRegistry.GetClassificationType(ClassificationTypeNames.MultiLineComment);
            _workspace = workspace;
        }

        #endregion

        #region Events

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        #endregion

        #region Methods

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0) {
                return Empty.Array<ITagSpan<IClassificationTag>>();
            }

            var tags = new List<ITagSpan<IClassificationTag>>();

            foreach (var span in spans) {
                var snapshot = span.Snapshot;

                if (_context.IsEmpty || _context.Snapshot != snapshot) {
                    var task = MultiLineCommentTaggerContext.CreateAsync(snapshot);
                    task.Wait();
                    if (task.IsFaulted) {
                        continue;
                    }
                    _context = task.Result;
                }

                var comments = GetCommentsInSnapshotSpan(span);
                foreach (var comment in comments) {
                    var token = GetTokenFromClassifiedSpan(comment);

                    if (token == null) {
                        continue;
                    }

                    var allTrivia = token.GetAllTrivia();
                    foreach (var trivia in allTrivia) {
                        if (trivia.IsKind(CS.SyntaxKind.MultiLineCommentTrivia)) {
                            tags.Add(CreateBlockCommentTag(snapshot, trivia.Span));
                        }
                    }
                }
            }

            return tags;
        }

        #endregion

        #region Methods: Implementation

        private ITagSpan<IClassificationTag> CreateBlockCommentTag(ITextSnapshot snapshot, TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(_blockCommentType));
        }

        private IList<ClassifiedSpan> GetCommentsInSnapshotSpan(SnapshotSpan span)
        {
            // Get classified spans
            var textSpan = TextSpan.FromBounds(span.Start, span.End);
            var classifiedSpans = Classifier.GetClassifiedSpans(_context.SemanticModel, textSpan, _workspace);

            // Filter identifiers
            var commentSpans = new List<ClassifiedSpan>();
            foreach (var classifiedSpan in classifiedSpans) {
                if (classifiedSpan.ClassificationType.Equals(
                    KnownClassificationTypeNames.Comment,
                    StringComparison.InvariantCultureIgnoreCase)) {
                    commentSpans.Add(classifiedSpan);
                }
            }

            return commentSpans;
        }

        private SyntaxToken GetTokenFromClassifiedSpan(ClassifiedSpan span)
        {
            return _context.SyntaxRoot.FindToken(span.TextSpan.Start);
        }

        #endregion
    }
}
