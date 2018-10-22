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

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;
using System.Threading;

#endregion

namespace VSEssentials.CommentFormatter
{
    internal sealed class CommentFormatter
    {
        #region Fields

        private readonly IClassificationFormatMap _formatMap;
        private readonly IWpfTextView _textView;
        private readonly IClassificationTypeRegistryService _typeRegistry;

        #endregion

        #region Constructors

        internal CommentFormatter(
            IClassificationFormatMap formatMap,
            IWpfTextView textView,
            IClassificationTypeRegistryService typeRegistryService)
        {
            // Initialize instance fields
            _formatMap = formatMap;
            _textView = textView;
            _typeRegistry = typeRegistryService;

            // Subscribe to events
            CommentFormatterOptions.Instance.PropertyChanged += Options_PropertyChanged;
            FormatMap.ClassificationFormatMappingChanged += FormatMap_ClassificationFormatMappingChanged;
            TextView.GotAggregateFocus += TextView_GotAggregateFocus;
            TextView.Closed += TextView_Closed;
        }

        #endregion

        #region Properties

        internal IClassificationFormatMap FormatMap {
            get { return _formatMap; }
        }

        internal IWpfTextView TextView {
            get { return _textView; }
        }

        internal IClassificationTypeRegistryService TypeRegistryService {
            get { return _typeRegistry; }
        }

        #endregion

        #region Methods

        private void InvalidateFormatting(CommentFormatterActions actions)
        {
            // Format ordinary comments
            if ((actions & CommentFormatterActions.FormatComments) ==
                CommentFormatterActions.FormatComments) {
                // Get classification type name for comments
                foreach (var name in KnownClassificationTypeNames.Comments) {
                    // Get the corresponding classification type
                    var ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        // Format
                        if (CommentFormatterOptions.Instance.ItalicizeComments) {
                            ClassificationFormatter.Italicize(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Normalize(FormatMap, ct);
                        }
                    }
                }
            }

            // Format documentation comments
            if ((actions & CommentFormatterActions.FormatDocumentationComments) ==
                CommentFormatterActions.FormatDocumentationComments) {
                // Get classification type name for documentation comments
                foreach (var name in KnownClassificationTypeNames.DocumentationComments) {
                    // Get the corresponding classification type
                    var ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        // Format
                        if (CommentFormatterOptions.Instance.ItalicizeDocumentationComments) {
                            ClassificationFormatter.Italicize(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Normalize(FormatMap, ct);
                        }
                    }
                }
            }

            // Format documentation tags
            if ((actions & CommentFormatterActions.FormatDocumentationTags) ==
                CommentFormatterActions.FormatDocumentationTags) {
                // Get classification type name for documentation tags
                foreach (var name in KnownClassificationTypeNames.DocumentationTags) {
                    // Get the corresponding classification type
                    var ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        // Format
                        if (CommentFormatterOptions.Instance.FadeDocumentationTags) {
                            ClassificationFormatter.Fade(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Sharpen(FormatMap, ct);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods: Event Handler

        private void FormatMap_ClassificationFormatMappingChanged(Object sender, EventArgs e)
        {
            // Invalidate formatting on format map changes
            InvalidateFormatting(CommentFormatterActions.All);
        }

        private void Options_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            // Validate arguments
            if (e == null) {
                return;
            }

            // Update formatting depending on the property changed
            switch (e.PropertyName) {
                case nameof(CommentFormatterOptions.ItalicizeComments):
                    InvalidateFormatting(CommentFormatterActions.FormatComments);
                    break;
                case nameof(CommentFormatterOptions.ItalicizeDocumentationComments):
                    InvalidateFormatting(CommentFormatterActions.FormatDocumentationComments);
                    break;
                case nameof(CommentFormatterOptions.FadeDocumentationTags):
                    InvalidateFormatting(CommentFormatterActions.FormatDocumentationTags);
                    break;
                default:
                    InvalidateFormatting(CommentFormatterActions.All);
                    break;
            }
        }

        private void TextView_Closed(Object sender, EventArgs e)
        {
            // Remove text view event handlers
            if (TextView != null) {
                TextView.Closed -= TextView_Closed;
                TextView.GotAggregateFocus -= TextView_GotAggregateFocus;
            }

            // Remove format map event handlers
            if (FormatMap != null) {
                FormatMap.ClassificationFormatMappingChanged -= FormatMap_ClassificationFormatMappingChanged;
            }

            // Remove options event handler
            CommentFormatterOptions.Instance.PropertyChanged -= Options_PropertyChanged;
        }

        private void TextView_GotAggregateFocus(Object sender, EventArgs e)
        {
            // Remove aggregate focus event handler
            if (TextView != null) {
                TextView.GotAggregateFocus -= TextView_GotAggregateFocus;
            }

            // Invalidate formatting on getting aggregate focus
            InvalidateFormatting(CommentFormatterActions.All);
        }

        #endregion
    }
}
