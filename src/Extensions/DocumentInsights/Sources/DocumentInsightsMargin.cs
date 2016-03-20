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

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Text;

#endregion

namespace VSEssentials.DocumentInsights
{
    internal sealed class DocumentInsightsMargin : IWpfTextViewMargin
    {
        #region Constants

        public const String MarginName = "Document Insights Margin";

        #endregion

        #region Fields

        private readonly ITextDocument _textDocument;
        private readonly IWpfTextView _textView;
        private readonly DocumentInsightsView _view;
        private readonly DocumentInsightsViewModel _viewModel;
        private Boolean _disposed;

        #endregion

        #region Constructors

        public DocumentInsightsMargin(IWpfTextView textView, ITextDocumentFactoryService documentService)
        {
            Int32 sourceLineCount = 0;
            Int32 sourceCharCount = 0;

            // Instance initialization
            _textView = textView;

            if (documentService.TryGetTextDocument(textView.TextBuffer, out _textDocument)) {
                sourceLineCount = _textDocument.TextBuffer.CurrentSnapshot.LineCount;
                sourceCharCount = _textDocument.TextBuffer.CurrentSnapshot.Length;
                _textDocument.FileActionOccurred += TextDocument_FileActionOccurred;
            }

            Encoding encoding = _textDocument != null ? _textDocument.Encoding : null;

            _viewModel = new DocumentInsightsViewModel(sourceLineCount, sourceCharCount, encoding) {
                ShowCharInfo = DocumentInsightsOptions.Current.EnableCharInfo,
                ShowLineInfo = DocumentInsightsOptions.Current.EnableLineInfo,
                ShowEncodingInfo = DocumentInsightsOptions.Current.EnableEncodingInfo
            };

            _view = new DocumentInsightsView() {
                DataContext = _viewModel,
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
                if (_disposed) {
                    throw new ObjectDisposedException(MarginName);
                }
                return true;
            }
        }

        public Double MarginSize {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException(MarginName);
                }
                return _view.ActualHeight;
            }
        }

        public FrameworkElement VisualElement {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException(MarginName);
                }
                return _view;
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (!_disposed) {
                _disposed = true;
                GC.SuppressFinalize(this);

                // Unsubscribe options property changed notifications
                DocumentInsightsOptions.Current.PropertyChanged -= Options_PropertyChanged;

                // Remove event handlers
                if (_textDocument != null) {
                    _textDocument.FileActionOccurred -= TextDocument_FileActionOccurred;
                }

                if (_textView != null) {
                    if (_textView.TextBuffer != null) {
                        _textView.TextBuffer.Changed -= TextBuffer_Changed;
                    }
                    _textView.Closed -= TextView_Closed;
                }
            }
        }

        public ITextViewMargin GetTextViewMargin(String marginName)
        {
            return String.Equals(marginName, MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        #endregion

        #region Methods: Event Handler

        private void Options_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            DocumentInsightsOptions current = DocumentInsightsOptions.Current;

            switch (e.PropertyName) {
                case nameof(DocumentInsightsOptions.ShowMargin):
                    _view.Visibility = current.ShowMargin ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(DocumentInsightsOptions.EnableLineInfo):
                    _viewModel.ShowLineInfo = current.EnableLineInfo;
                    break;
                case nameof(DocumentInsightsOptions.EnableCharInfo):
                    _viewModel.ShowCharInfo = current.EnableCharInfo;
                    break;
                case nameof(DocumentInsightsOptions.EnableEncodingInfo):
                    _viewModel.ShowEncodingInfo = current.EnableEncodingInfo;
                    break;
            }
        }

        private void TextBuffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            ITextBuffer textBuffer = sender as ITextBuffer;

            if (textBuffer != null) {
                _viewModel.ActualLineCount = textBuffer.CurrentSnapshot.LineCount;
                _viewModel.ActualCharCount = textBuffer.CurrentSnapshot.Length;
            }
        }

        private void TextDocument_FileActionOccurred(Object sender, TextDocumentFileActionEventArgs e)
        {
            ITextDocument textDocument = sender as ITextDocument;

            if (textDocument != null) {
                if (_viewModel != null) {
                    _viewModel.Encoding = textDocument.Encoding;
                }
            }
        }

        private void TextView_Closed(Object sender, EventArgs e)
        {
            Dispose();
        }

        #endregion
    }
}
