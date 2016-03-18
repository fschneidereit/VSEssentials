/***************************************************************************************************
 *
 *  Copyright © 2016 Florian Schneidereit
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
using CSharp = Microsoft.CodeAnalysis.CSharp;

#endregion

namespace VSEssentials.SemanticFormatter
{
    internal class SemanticFormatter : ITagger<IClassificationTag>
    {
        #region Fields

        private readonly IClassificationType _fieldIdentifierType;
        private readonly VisualStudioWorkspace _workspace;

        private SemanticFormatterContext _context;

        #endregion

        #region Constructors

        public SemanticFormatter(VisualStudioWorkspace workspace, IClassificationTypeRegistryService classificationTypeRegistry)
        {
            // Initialize instance
            _fieldIdentifierType = classificationTypeRegistry.GetClassificationType(ClassificationTypeNames.FieldIdentifier);
            _workspace = workspace;
        }

        #endregion

        #region Events

#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        #endregion

        #region Properties

        internal SemanticFormatterContext Context {
            get { return _context; }
            private set { _context = value; }
        }

        internal IClassificationType FieldIdentifierType {
            get { return _fieldIdentifierType; }
        }

        internal VisualStudioWorkspace Workspace {
            get { return _workspace; }
        }

        #endregion

        #region Methods

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0) {
                return Empty<ITagSpan<IClassificationTag>>.Array;
            }

            var snapshot = spans[0].Snapshot;

            if (Context == null || Context.TextSnapshot != snapshot) {
                var task = SemanticFormatterContext.CreateAsync(snapshot);
                task.Wait();
                if (task.IsFaulted) {
                    return Empty<ITagSpan<IClassificationTag>>.Array;
                }
                Context = task.Result;
            }

            var identifiers = GetIdentifiersInSnapshotSpans(spans);
            var tags = new List<TagSpan<IClassificationTag>>();

            foreach (var identifier in identifiers) {
                var node = Context.SyntaxRoot.FindNode(identifier.TextSpan);
                var symbol = Context.SemanticModel.GetSymbolInfo(node).Symbol;
                if (symbol == null) {
                    continue;
                }
                switch (symbol.Kind) {
                    case SymbolKind.Field:
                        if (symbol.ContainingType.TypeKind != TypeKind.Enum) {
                            tags.Add(new TagSpan<IClassificationTag>(
                                new SnapshotSpan(snapshot, identifier.TextSpan.Start, identifier.TextSpan.Length),
                                new ClassificationTag(FieldIdentifierType)));
                        }
                        break;
                }
            }

            return tags;
        }

        #endregion

        #region Methods: Implementation

        private IEnumerable<ClassifiedSpan> GetIdentifiersInSnapshotSpans(NormalizedSnapshotSpanCollection spans)
        {
            // Get classified spans
            List<ClassifiedSpan> classifiedSpans = new List<ClassifiedSpan>();

            foreach (SnapshotSpan span in spans) {
                var textSpan = TextSpan.FromBounds(span.Start, span.End);
                classifiedSpans.AddRange(Classifier.GetClassifiedSpans(Context.SemanticModel, textSpan, Workspace));
            }

            // Filter identifiers
            List<ClassifiedSpan> identifierSpans = new List<ClassifiedSpan>();

            foreach (ClassifiedSpan classifiedSpan in classifiedSpans) {
                if (classifiedSpan.ClassificationType.Equals(KnownClassificationTypeNames.Identifier, StringComparison.InvariantCultureIgnoreCase)) {
                    identifierSpans.Add(classifiedSpan);
                }
            }

            return identifierSpans;
        }

        #endregion
    }
}
