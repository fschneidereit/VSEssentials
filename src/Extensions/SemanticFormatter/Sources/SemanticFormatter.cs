﻿/***************************************************************************************************
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
using Microsoft.VisualStudio.Shell;
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
        private readonly IClassificationType _extensionMethodIdentifierType;
        private readonly IClassificationType _fieldIdentifierType;
        private readonly IClassificationType _ordinaryMethodIdentifierType;
        private readonly Workspace _workspace;

        #endregion

        #region Constructors

        public SemanticFormatter(
            Workspace workspace,
            IClassificationTypeRegistryService classificationTypeRegistry)
        {
            // Initialize instance
            _context = SemanticFormatterContext.Empty;
            _extensionMethodIdentifierType =
                classificationTypeRegistry.GetClassificationType(
                    ClassificationTypeNames.ExtensionMethodIdentifier);
            _fieldIdentifierType =
                classificationTypeRegistry.GetClassificationType(
                    ClassificationTypeNames.FieldIdentifier);
            _ordinaryMethodIdentifierType =
                classificationTypeRegistry.GetClassificationType(
                    ClassificationTypeNames.OrdinaryMethodIdentifier);
            _workspace = workspace;
        }

        #endregion

        #region Events

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        #endregion

        #region Methods

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(
            NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0) {
                yield break;
            }

            var snapshot = spans[0].Snapshot;

            if (_context.IsEmpty || _context.Snapshot != snapshot) {
                try {
                    _context = ThreadHelper.JoinableTaskFactory.Run(
                        () => SemanticFormatterContext.CreateAsync(snapshot));
                } catch {
                    _context = SemanticFormatterContext.Empty;
                    yield break;
                }
            }

            var identifiers = GetIdentifiersInSnapshotSpan(spans[0]);
            foreach (var identifier in identifiers) {
                var node = GetNodeFromTextSpan(identifier.TextSpan);
                if (node.IsPartOfStructuredTrivia()) {
                    continue;
                }

                var symbol = GetSymbolFromNode(node);
                if (symbol == null) {
                    continue;
                }

                switch (symbol.Kind) {
                    case SymbolKind.Field:
                        if (symbol.ContainingType.TypeKind != TypeKind.Enum) {
                            yield return CreateFieldIdentifierTag(snapshot, identifier.TextSpan);
                        }
                        break;
                    case SymbolKind.Method:
                        var methodSymbol = symbol as IMethodSymbol;
                        if (methodSymbol != null) {
                            if (methodSymbol.MethodKind == MethodKind.Ordinary) {
                                yield return CreateOrdinaryMethodIdentifierTag(
                                    snapshot,
                                    identifier.TextSpan);
                            }
                            if (methodSymbol.IsExtensionMethod) {
                                yield return CreateExtensionMethodIdentifierTag(
                                    snapshot,
                                    identifier.TextSpan);
                            }
                        }
                        break;
                }
            }
        }

        private void OnTagsChanged(SnapshotSpanEventArgs e)
        {
            TagsChanged?.Invoke(this, e);
        }

        #endregion

        #region Methods: Implementation

        private ITagSpan<IClassificationTag> CreateExtensionMethodIdentifierTag(
            ITextSnapshot snapshot,
            TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(_extensionMethodIdentifierType));
        }

        private ITagSpan<IClassificationTag> CreateFieldIdentifierTag(
            ITextSnapshot snapshot,
            TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(_fieldIdentifierType));
        }

        private ITagSpan<IClassificationTag> CreateOrdinaryMethodIdentifierTag(
            ITextSnapshot snapshot,
            TextSpan span)
        {
            return new TagSpan<IClassificationTag>(
                new SnapshotSpan(snapshot, span.Start, span.Length),
                new ClassificationTag(_ordinaryMethodIdentifierType));
        }

        private IEnumerable<ClassifiedSpan> GetIdentifiersInSnapshotSpan(SnapshotSpan span)
        {
            // Get classified spans
            var textSpan = TextSpan.FromBounds(span.Start, span.End);
            var classifiedSpans =
                Classifier.GetClassifiedSpans(_context.SemanticModel, textSpan, _workspace);

            // Filter identifiers
            foreach (var classifiedSpan in classifiedSpans) {
                if (classifiedSpan.ClassificationType.Equals(
                    KnownClassificationTypeNames.Identifier,
                    StringComparison.InvariantCultureIgnoreCase)) {
                    yield return classifiedSpan;
                }
            }
        }

        private SyntaxNode GetNodeFromTextSpan(TextSpan span)
        {
            return _context.SyntaxRoot.FindNode(span, true);
        }

        private ISymbol GetSymbolFromNode(SyntaxNode node)
        {
            // CS/VB Argument Syntax
            if (node is CS.Syntax.ArgumentSyntax) {
                var expression = ((CS.Syntax.ArgumentSyntax)node).Expression;
                return _context.SemanticModel.GetSymbolInfo(expression).Symbol;
            }

            if (node is VB.Syntax.SimpleArgumentSyntax) {
                var expression = ((VB.Syntax.SimpleArgumentSyntax)node).Expression;
                return _context.SemanticModel.GetSymbolInfo(expression).Symbol;
            }

            var symbol = _context.SemanticModel.GetSymbolInfo(node).Symbol;
            return symbol != null ? symbol : _context.SemanticModel.GetDeclaredSymbol(node);
        }

        #endregion
    }
}
