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
using CS = Microsoft.CodeAnalysis.CSharp;
using VB = Microsoft.CodeAnalysis.VisualBasic;

#endregion

namespace VSEssentials.SemanticFormatter
{
    internal class SemanticFormatter : ITagger<IClassificationTag>
    {
        #region Fields

        private SemanticFormatterContext _context;
        private readonly IClassificationType _fieldIdentifierType;
        private readonly VisualStudioWorkspace _workspace;

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
                return Empty.Array<ITagSpan<IClassificationTag>>();
            }

            var snapshot = spans[0].Snapshot;

            if (Context == null || Context.TextSnapshot != snapshot) {
                var task = SemanticFormatterContext.CreateAsync(snapshot);
                task.Wait();
                if (task.IsFaulted) {
                    return Empty.Array<ITagSpan<IClassificationTag>>();
                }
                Context = task.Result;
            }

            var identifiers = GetIdentifiersInSnapshotSpans(spans);
            var tags = new List<ITagSpan<IClassificationTag>>();

            foreach (var identifier in identifiers) {
                var node = Context.SyntaxRoot.FindNode(identifier.TextSpan);
                var symbol = GetSymbolFromNode(node);

                if (symbol == null) {
                    continue;
                }

                if (symbol.Kind == SymbolKind.Field) {
                    if (symbol.ContainingType.TypeKind != TypeKind.Enum) {
                        tags.Add(CreateFieldIdentifierTag(snapshot, identifier.TextSpan));
                    }
                }
            }

            return tags;
        }

        #endregion

        #region Methods: Implementation

        private ITagSpan<IClassificationTag> CreateFieldIdentifierTag(ITextSnapshot snapshot, TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(FieldIdentifierType));
        }

        private IEnumerable<ClassifiedSpan> GetIdentifiersInSnapshotSpans(NormalizedSnapshotSpanCollection spans)
        {
            // Get classified spans
            var classifiedSpans = new List<ClassifiedSpan>();
            foreach (var span in spans) {
                var textSpan = TextSpan.FromBounds(span.Start, span.End);
                classifiedSpans.AddRange(Classifier.GetClassifiedSpans(Context.SemanticModel, textSpan, Workspace));
            }

            // Filter identifiers
            var identifierSpans = new List<ClassifiedSpan>();
            foreach (var classifiedSpan in classifiedSpans) {
                if (classifiedSpan.ClassificationType.Equals(
                    KnownClassificationTypeNames.Identifier,
                    StringComparison.InvariantCultureIgnoreCase)) {
                    identifierSpans.Add(classifiedSpan);
                }
            }

            return identifierSpans;
        }

        private ISymbol GetSymbolFromNode(SyntaxNode node)
        {
            if ((node is CS.Syntax.VariableDeclaratorSyntax) || (node is VB.Syntax.VariableDeclaratorSyntax)) {
                return Context.SemanticModel.GetDeclaredSymbol(node);
            }

            return Context.SemanticModel.GetSymbolInfo(node).Symbol;
        }

        #endregion
    }
}
