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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

#endregion

namespace VSEssentials.CommentFormatter
{
    [StructLayout(LayoutKind.Auto)]
    internal struct MultiLineCommentTaggerContext
    {
        public static readonly MultiLineCommentTaggerContext Empty;

        #region Fields

        private readonly Document _document;
        private readonly SemanticModel _semanticModel;
        private readonly SyntaxNode _syntaxRoot;
        private readonly ITextSnapshot _snapshot;

        #endregion

        #region Constructors

        public MultiLineCommentTaggerContext(
            Document document = null,
            SemanticModel semanticModel = null,
            SyntaxNode syntaxRoot = null,
            ITextSnapshot snapshot = null)
        {
            // Instance initialization
            _document = document;
            _semanticModel = semanticModel;
            _syntaxRoot = syntaxRoot;
            _snapshot = snapshot;
        }

        #endregion

        #region Properties

        public Document Document {
            get { return _document; }
        }

        public Boolean IsEmpty {
            get { return Document == null && SemanticModel == null && SyntaxRoot == null && Snapshot == null; }
        }

        public SemanticModel SemanticModel {
            get { return _semanticModel; }
        }

        public ITextSnapshot Snapshot {
            get { return _snapshot; }
        }

        public SyntaxNode SyntaxRoot {
            get { return _syntaxRoot; }
        }

        #endregion

        #region Methods: Static

        public static async Task<MultiLineCommentTaggerContext> CreateAsync(ITextSnapshot snapshot)
        {
            var document = snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var semanticModel = await document.GetSemanticModelAsync();
            var syntaxRoot = await document.GetSyntaxRootAsync();

            return new MultiLineCommentTaggerContext(document, semanticModel, syntaxRoot, snapshot);
        }

        #endregion
    }
}
