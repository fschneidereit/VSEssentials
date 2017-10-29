/***************************************************************************************************
 *
 *  Copyright © 2015-2017 Florian Schneidereit
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

using VSEssentials.Common;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;

#endregion

namespace VSEssentials.CommentFormatter
{
    public class CommentFormatterOptionPage : DialogPage
    {
        #region Constants

        public const String CategoryName = "Extensions";
        public const String PageName = @"VSEssentials\Comment Formatter";

        #endregion

        #region Properties

        [Category("Behavior")]
        [LocalizableDisplayName(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.FadeDocumentationTagsOptionDisplayName)]
        [LocalizableDescription(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.FadeDocumentationTagsOptionDescription)]
        [DefaultValue(true)]
        [TypeConverter(typeof(BooleanOptionConverter))]
        public Boolean FadeDocumentationTags {
            get { return CommentFormatterOptions.Instance.FadeDocumentationTags; }
            set { CommentFormatterOptions.Instance.FadeDocumentationTags = value; }
        }

        [Category("Behavior")]
        [LocalizableDisplayName(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.ItalicizeCommentsOptionDisplayName)]
        [LocalizableDescription(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.ItalicizeCommentsOptionDescription)]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanOptionConverter))]
        public Boolean ItalicizeComments {
            get { return CommentFormatterOptions.Instance.ItalicizeComments; }
            set { CommentFormatterOptions.Instance.ItalicizeComments = value; }
        }

        [Category("Behavior")]
        [LocalizableDisplayName(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.ItalicizeDocumentationCommentsOptionDisplayName)]
        [LocalizableDescription(
            LocalizationProvider = typeof(CommentFormatterLocalizationProvider),
            ResourceName = CommentFormatterLocalizedResourceNames.ItalicizeDocumentationCommentsOptionDescription)]
        [DefaultValue(true)]
        [TypeConverter(typeof(BooleanOptionConverter))]
        public Boolean ItalicizeDocumentationComments {
            get { return CommentFormatterOptions.Instance.ItalicizeDocumentationComments; }
            set { CommentFormatterOptions.Instance.ItalicizeDocumentationComments = value; }
        }

        #endregion
    }
}
