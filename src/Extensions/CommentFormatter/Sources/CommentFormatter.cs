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

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;

#endregion

namespace VSEssentials.CommentFormatter
{
    internal sealed class CommentFormatter
    {
        #region Fields

        private Boolean _formatting;
        private readonly IClassificationFormatMap _formatMap;
        private readonly IWpfTextView _textView;
        private readonly IClassificationTypeRegistryService _typeRegistry;

        #endregion

        #region Constructors

        internal CommentFormatter(IClassificationFormatMap formatMap, IWpfTextView textView, IClassificationTypeRegistryService typeRegistryService)
        {
            // Initialize instance fields
            _formatMap = formatMap;
            _textView = textView;
            _typeRegistry = typeRegistryService;

            // Initial formatting
            InvalidateFormatting();

            // Subscribe to events
            CommentFormatterOptions.Current.PropertyChanged += Options_PropertyChanged;
            FormatMap.ClassificationFormatMappingChanged += FormatMap_ClassificationFormatMappingChanged;
            TextView.GotAggregateFocus += TextView_GotAggregateFocus;
            TextView.Closed += TextView_Closed;
        }

        #endregion

        #region Properties

        Boolean CanFormat {
            get { return !IsFormatting || FormatMap != null || (TextView != null && !TextView.IsClosed); }
        }

        IClassificationFormatMap FormatMap {
            get { return _formatMap; }
        }

        Boolean IsFormatting {
            get { return _formatting; }
            set { _formatting = value; }
        }

        IWpfTextView TextView {
            get { return _textView; }
        }

        IClassificationTypeRegistryService TypeRegistryService {
            get { return _typeRegistry; }
        }

        #endregion

        #region Methods

        void FormatComments()
        {
            // Validate state
            if (!CanFormat) {
                return;
            }

            Boolean batchUpdate = false;

            try {
                // Begin formatting
                IsFormatting = true;

                if (!FormatMap.IsInBatchUpdate) {
                    batchUpdate = true;
                    FormatMap.BeginBatchUpdate();
                }

                // Process comments
                foreach (String name in KnownClassificationTypeNames.Comments) {
                    IClassificationType ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        if (CommentFormatterOptions.Current.ItalicizeComments) {
                            ClassificationFormatter.Italicize(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Normalize(FormatMap, ct);
                        }
                    }
                }
            } finally {
                // End formatting
                if (batchUpdate && FormatMap.IsInBatchUpdate) {
                    FormatMap.EndBatchUpdate();
                }

                IsFormatting = false;
            }
        }

        void FormatDocumentationComments()
        {
            // Validate state
            if (!CanFormat) {
                return;
            }

            Boolean batchUpdate = false;

            try {
                // Begin formatting
                IsFormatting = true;

                if (!FormatMap.IsInBatchUpdate) {
                    batchUpdate = true;
                    FormatMap.BeginBatchUpdate();
                }

                // Process documentation comments
                foreach (String name in KnownClassificationTypeNames.DocumentationComments) {
                    IClassificationType ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        if (CommentFormatterOptions.Current.ItalicizeDocumentationComments) {
                            ClassificationFormatter.Italicize(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Normalize(FormatMap, ct);
                        }
                    }
                }
            } finally {
                // End formatting
                if (batchUpdate && FormatMap.IsInBatchUpdate) {
                    FormatMap.EndBatchUpdate();
                }

                IsFormatting = false;
            }
        }

        void FormatDocumentationTags()
        {
            // Validate state
            if (!CanFormat) {
                return;
            }

            Boolean batchUpdate = false;

            try {
                // Begin formatting
                IsFormatting = true;

                if (!FormatMap.IsInBatchUpdate) {
                    batchUpdate = true;
                    FormatMap.BeginBatchUpdate();
                }

                // Process documentation tags
                foreach (String name in KnownClassificationTypeNames.DocumentationTags) {
                    IClassificationType ct = TypeRegistryService.GetClassificationType(name);
                    if (ct != null) {
                        if (CommentFormatterOptions.Current.FadeDocumentationTags) {
                            ClassificationFormatter.Fade(FormatMap, ct);
                        } else {
                            ClassificationFormatter.Sharpen(FormatMap, ct);
                        }
                    }
                }
            } finally {
                // End formatting
                if (batchUpdate && FormatMap.IsInBatchUpdate) {
                    FormatMap.EndBatchUpdate();
                }

                IsFormatting = false;
            }
        }

        void InvalidateFormatting()
        {
            // Perform all formattings
            FormatComments();
            FormatDocumentationComments();
            FormatDocumentationTags();
        }

        #endregion

        #region Methods: Event Handler

        private void FormatMap_ClassificationFormatMappingChanged(Object sender, EventArgs e)
        {
            // Invalidate formatting on classification map change
            InvalidateFormatting();
        }

        private void Options_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            // Validate event arguments
            if (e == null) {
                // Invalidate formatting if no arguments are provided
                InvalidateFormatting();
            }

            switch (e.PropertyName) {
                case nameof(CommentFormatterOptions.ItalicizeComments):
                    FormatComments();
                    break;
                case nameof(CommentFormatterOptions.ItalicizeDocumentationComments):
                    FormatDocumentationComments();
                    break;
                case nameof(CommentFormatterOptions.FadeDocumentationTags):
                    FormatDocumentationTags();
                    break;
                default:
                    // Invalidate formatting by default
                    InvalidateFormatting();
                    break;
            }
        }

        private void TextView_Closed(Object sender, EventArgs e)
        {
            if (TextView != null) {
                TextView.Closed -= TextView_Closed;
                TextView.GotAggregateFocus -= TextView_GotAggregateFocus;
            }

            if (FormatMap != null) {
                FormatMap.ClassificationFormatMappingChanged -= FormatMap_ClassificationFormatMappingChanged;
            }

            CommentFormatterOptions.Current.PropertyChanged -= Options_PropertyChanged;
        }

        private void TextView_GotAggregateFocus(Object sender, EventArgs e)
        {
            if (TextView != null) {
                TextView.GotAggregateFocus -= TextView_GotAggregateFocus;
            }

            // Invalidate formatting on getting aggregate focus
            InvalidateFormatting();
        }

        #endregion
    }
}
