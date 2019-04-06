/***************************************************************************************************
 *
 *  Copyright © 2015-2018 Florian Schneidereit
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
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Threading;
using CS = Microsoft.CodeAnalysis.CSharp;

#endregion

namespace VSEssentials.CommentFormatter
{
    internal class MultiLineCommentTagger : ITagger<IClassificationTag>
    {
        #region Fields

        private MultiLineCommentTaggerContext _context;
        private readonly IClassificationType _multiLineCommentType;
        private readonly ITextBuffer _textBuffer;
        private readonly Workspace _workspace;

        #endregion

        #region Constructors

        public MultiLineCommentTagger(
            ITextBuffer textBuffer,
            Workspace workspace,
            IClassificationTypeRegistryService classificationTypeRegistry)
        {
            // Instance initialization
            _context = MultiLineCommentTaggerContext.Empty;
            _multiLineCommentType =
                classificationTypeRegistry.GetClassificationType(
                    ClassificationTypeNames.MultiLineComment);
            _textBuffer = textBuffer;
            _workspace = workspace;
        }

        #endregion

        #region Events

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        #endregion

        #region Methods

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(
            NormalizedSnapshotSpanCollection spans)
        {
            // Break if there are no spans
            if (spans.Count == 0) {
                yield break;
            }

            // Get text snapshot of the first span
            var snapshot = spans[0].Snapshot;

            if (_context.IsEmpty || _context.Snapshot != snapshot) {
                try {
                    _context = ThreadHelper.JoinableTaskFactory.Run(
                        () => MultiLineCommentTaggerContext.CreateAsync(snapshot));
                } catch {
                    _context = MultiLineCommentTaggerContext.Empty;
                    yield break;
                }
            }

            var comments = GetCommentsInSnapshotSpan(spans[0]);
            foreach (var comment in comments) {
                var token = GetTokenFromClassifiedSpan(comment);
                if (token == null) {
                    continue;
                }

                var allTrivia = token.GetAllTrivia();
                foreach (var trivia in allTrivia) {
                    if (trivia.IsKind(CS.SyntaxKind.MultiLineCommentTrivia)) {
                        yield return CreateMultiLineCommentTag(snapshot, trivia.Span);
                    }
                }
            }
        }

        #endregion

        #region Methods: Implementation

        private ITagSpan<IClassificationTag> CreateMultiLineCommentTag(
            ITextSnapshot snapshot,
            TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(_multiLineCommentType));
        }

        private IEnumerable<ClassifiedSpan> GetCommentsInSnapshotSpan(SnapshotSpan span)
        {
            // Check if the span is empty
            if (span.IsEmpty) {
                yield break;
            }

            // Get classified spans
            var textSpan = TextSpan.FromBounds(span.Start, span.End);
            var classifiedSpans =
                Classifier.GetClassifiedSpans(_context.SemanticModel, textSpan, _workspace);

            if (classifiedSpans != null) {
                // Filter identifiers
                foreach (var classifiedSpan in classifiedSpans) {
                    if (classifiedSpan.ClassificationType.Equals(
                        KnownClassificationTypeNames.Comment,
                        StringComparison.InvariantCultureIgnoreCase)) {
                        yield return classifiedSpan;
                    }
                }
            } else {
                yield break;
            }
        }

        private SyntaxToken GetTokenFromClassifiedSpan(ClassifiedSpan span)
        {
            return _context.SyntaxRoot.FindToken(span.TextSpan.Start);
        }

        #endregion
    }
}
