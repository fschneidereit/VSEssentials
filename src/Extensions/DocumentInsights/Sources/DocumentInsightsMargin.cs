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

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;
using System.Windows;

#endregion

namespace Flatcode.VSEssentials.Extensions.DocumentInsights
{
    sealed class DocumentInsightsMargin : IWpfTextViewMargin
    {
        #region Constants

        public const String MarginName = "Document Insights Margin";

        #endregion

        #region Fields

        readonly ITextDocument textDocument;
        readonly IWpfTextView textView;
        readonly DocumentInsightsView view;
        readonly DocumentInsightsViewModel viewModel;
        Boolean disposed;

        #endregion

        #region Constructors

        public DocumentInsightsMargin(IWpfTextView textView, ITextDocumentFactoryService documentService)
        {
            Int32 sourceLineCount = 0;
            Int32 sourceCharCount = 0;

            // Instance initialization
            this.textView = textView;

            if (documentService.TryGetTextDocument(textView.TextBuffer, out textDocument)) {
                sourceLineCount = textDocument.TextBuffer.CurrentSnapshot.LineCount;
                sourceCharCount = textDocument.TextBuffer.CurrentSnapshot.Length;
            }

            viewModel = new DocumentInsightsViewModel(sourceLineCount, sourceCharCount) {
                ShowCharInfo = DocumentInsightsOptions.Current.EnableCharInfo,
                ShowLineInfo = DocumentInsightsOptions.Current.EnableLineInfo
            };

            view = new DocumentInsightsView() {
                DataContext = viewModel,
                Visibility = DocumentInsightsOptions.Current.ShowMargin ? Visibility.Visible : Visibility.Collapsed
            };

            // Add event handler
            if (textView != null) {
                textView.Closed += TextView_Closed;
                if (textView.TextBuffer != null) {
                    textView.TextBuffer.Changed += TextBuffer_Changed;
                }
            }

            // Subscribe options property changed notifications
            DocumentInsightsOptions.Current.PropertyChanged += Options_PropertyChanged;
        }

        #endregion

        #region Properties

        public Boolean Enabled {
            get {
                if (disposed) {
                    throw new ObjectDisposedException(MarginName);
                }

                return true;
            }
        }

        public Double MarginSize {
            get {
                if (disposed) {
                    throw new ObjectDisposedException(MarginName);
                }
                return view.ActualHeight;
            }
        }

        public FrameworkElement VisualElement {
            get {
                if (disposed) {
                    throw new ObjectDisposedException(MarginName);
                }

                return view;
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (!disposed) {
                disposed = true;
                GC.SuppressFinalize(this);

                // Unsubscribe options property changed notifications
                DocumentInsightsOptions.Current.PropertyChanged -= Options_PropertyChanged;

                // Remove event handlers
                if (textView != null) {
                    if (textView.TextBuffer != null) {
                        textView.TextBuffer.Changed -= TextBuffer_Changed;
                    }
                    textView.Closed -= TextView_Closed;
                }
            }
        }

        public ITextViewMargin GetTextViewMargin(String marginName)
        {
            return String.Equals(marginName, MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        #endregion

        #region Methods: Event Handler

        void Options_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            DocumentInsightsOptions current = DocumentInsightsOptions.Current;

            switch (e.PropertyName) {
                case nameof(DocumentInsightsOptions.ShowMargin):
                    view.Visibility = current.ShowMargin ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(DocumentInsightsOptions.EnableLineInfo):
                    viewModel.ShowLineInfo = current.EnableLineInfo;
                    break;
                case nameof(DocumentInsightsOptions.EnableCharInfo):
                    viewModel.ShowCharInfo = current.EnableCharInfo;
                    break;
            }
        }

        void TextBuffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            ITextBuffer textBuffer = sender as ITextBuffer;

            if (textBuffer != null) {
                viewModel.ActualLineCount = textBuffer.CurrentSnapshot.LineCount;
                viewModel.ActualCharCount = textBuffer.CurrentSnapshot.Length;
            }
        }


        void TextView_Closed(Object sender, EventArgs e)
        {
            Dispose();
        }


        #endregion
    }
}
