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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

#endregion

namespace VSEssentials.SemanticFormatter
{
    [StructLayout(LayoutKind.Auto)]
    internal struct SemanticFormatterContext
    {
        public static readonly SemanticFormatterContext Empty = new SemanticFormatterContext(null, null, null, null);

        #region Fields

        private readonly Document _document;
        private readonly SemanticModel _semanticModel;
        private readonly SyntaxNode _syntaxRoot;
        private readonly ITextSnapshot _textSnapshot;

        #endregion

        #region Constructors

        internal SemanticFormatterContext(
            Document document,
            SemanticModel semanticModel,
            SyntaxNode syntaxRoot,
            ITextSnapshot textSnapshot)
        {
            _document = document;
            _semanticModel = semanticModel;
            _syntaxRoot = syntaxRoot;
            _textSnapshot = textSnapshot;
        }

        #endregion

        #region Properties

        public Document Document {
            get { return _document; }
        }

        public Boolean IsEmpty {
            get { return Document == null && SemanticModel == null && SyntaxRoot == null & TextSnapshot == null; }
        }

        public SemanticModel SemanticModel {
            get { return _semanticModel; }
        }

        public SyntaxNode SyntaxRoot {
            get { return _syntaxRoot; }
        }

        public ITextSnapshot TextSnapshot {
            get { return _textSnapshot; }
        }

        #endregion

        #region Methods: Static

        public static async Task<SemanticFormatterContext> CreateAsync(ITextSnapshot textSnapshot)
        {
            var document = textSnapshot.GetOpenDocumentInCurrentContextWithChanges();
            var semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);
            var syntaxRoot = await document.GetSyntaxRootAsync().ConfigureAwait(false);

            return new SemanticFormatterContext(document, semanticModel, syntaxRoot, textSnapshot);
        }

        #endregion
    }
}
