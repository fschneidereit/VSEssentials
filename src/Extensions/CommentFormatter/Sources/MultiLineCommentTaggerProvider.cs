﻿/***************************************************************************************************
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
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

#endregion

namespace VSEssentials.CommentFormatter
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(ContentTypes.CSharp)]
    [TagType(typeof(IClassificationTag))]
    internal sealed class MultiLineCommentTaggerProvider : ITaggerProvider
    {
        #region Fields

        [Import]
        private IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }
        [Import]
        private VisualStudioWorkspace Workspace { get; set; }

        #endregion

        #region Methods

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return (ITagger<T>)buffer.Properties.GetOrCreateSingletonProperty(
                () => new MultiLineCommentTagger(buffer, Workspace, ClassificationTypeRegistry));
        }

        #endregion
    }
}
