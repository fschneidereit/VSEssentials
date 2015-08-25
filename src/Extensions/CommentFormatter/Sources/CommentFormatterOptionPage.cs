/***************************************************************************************************
 *
 *  Copyright © 2015 Flatcode.net
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

using Flatcode.VSEssentials.Common;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;

#endregion

namespace Flatcode.VSEssentials.Extensions.CommentFormatter
{
    public class CommentFormatterOptionPage : DialogPage
    {
        #region Properties

        [Category("Behavior")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.FadeDocumentationTagsDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.FadeDocumentationTagsDescription)]
        [DefaultValue(true)]
        public Boolean FadeDocumentationTags {
            get { return CommentFormatterOptions.Current.FadeDocumentationTags; }
            set { CommentFormatterOptions.Current.FadeDocumentationTags = value; }
        }

        [Category("Behavior")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ItalicizeCommentsDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ItalicizeCommentsDescription)]
        [DefaultValue(false)]
        public Boolean ItalicizeComments {
            get { return CommentFormatterOptions.Current.ItalicizeComments; }
            set { CommentFormatterOptions.Current.ItalicizeComments = value; }
        }

        [Category("Behavior")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ItalicizeDocumentationCommentsDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ItalicizeDocumentationCommentsDescription)]
        [DefaultValue(true)]
        public Boolean ItalicizeDocumentationComments {
            get { return CommentFormatterOptions.Current.ItalicizeDocumentationComments; }
            set { CommentFormatterOptions.Current.ItalicizeDocumentationComments = value; }
        }

        #endregion
    }
}
