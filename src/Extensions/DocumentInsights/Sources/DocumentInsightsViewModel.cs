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
using System.ComponentModel;
using System.Text;

#endregion

namespace VSEssentials.DocumentInsights
{
    internal sealed class DocumentInsightsViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly Int32 _sourceCharCount;
        private readonly Int32 _sourceLineCount;
        private Int32 _actualCharCount;
        private Int32 _actualLineCount;
        private Encoding _encoding;
        private Boolean _showCharInfo;
        private Boolean _showLineInfo;
        private Boolean _showEncoding;

        #endregion

        #region Constructors

        public DocumentInsightsViewModel(Int32 lineCount = 0, Int32 charCount = 0, Encoding encoding = null)
        {
            // Instance initialization
            _actualLineCount = _sourceLineCount = lineCount;
            _actualCharCount = _sourceCharCount = charCount;
            _encoding = encoding;

            // Initialize defaults
            _showCharInfo = true;
            _showLineInfo = true;
            _showEncoding = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Int32 ActualCharCount {
            get { return _actualCharCount; }
            set {
                if (_actualCharCount != value) {
                    _actualCharCount = value;
                    OnPropertyChanged(nameof(ActualCharCount));
                    OnPropertyChanged(nameof(CharDifference));
                }
            }
        }

        public String ActualCharCountLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.ActualCharCountLabel); }
        }

        public Int32 ActualLineCount {
            get { return _actualLineCount; }
            set {
                if (_actualLineCount != value) {
                    _actualLineCount = value;
                    OnPropertyChanged(nameof(ActualLineCount));
                    OnPropertyChanged(nameof(LineDifference));
                }
            }
        }

        public String ActualLineCountLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.ActualLineCountLabel); }
        }

        public Int32 CharDifference {
            get { return ActualCharCount - SourceCharCount; }
        }

        public String CharInfoLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.CharInfoLabel); }
        }

        public String DifferenceLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.DifferenceLabel); }
        }

        public Encoding Encoding {
            get { return _encoding; }
            set {
                if (_encoding != value) {
                    _encoding = value;
                    OnPropertyChanged(nameof(Encoding));
                }
            }
        }

        public String EncodingInfoLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.EncodingInfoLabel); }
        }

        public Int32 LineDifference {
            get { return ActualLineCount - SourceLineCount; }
        }

        public String LineInfoLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.LineInfoLabel); }
        }

        public Boolean ShowCharInfo {
            get { return _showCharInfo; }
            set {
                if (_showCharInfo != value) {
                    _showCharInfo = value;
                    OnPropertyChanged(nameof(ShowCharInfo));
                }
            }
        }

        public Boolean ShowEncodingInfo {
            get { return _showEncoding; }
            set {
                if (_showEncoding != value) {
                    _showEncoding = value;
                    OnPropertyChanged(nameof(ShowEncodingInfo));
                }
            }
        }

        public Boolean ShowLineInfo {
            get { return _showLineInfo; }
            set {
                if (_showLineInfo != value) {
                    _showLineInfo = value;
                    OnPropertyChanged(nameof(ShowLineInfo));
                }
            }
        }

        public Int32 SourceCharCount {
            get { return _sourceCharCount; }
        }

        public String SourceCharCountLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.SourceCharCountLabel); }
        }

        public Int32 SourceLineCount {
            get { return _sourceLineCount; }
        }

        public String SourceLineCountLabel {
            get { return LocalLocalizationProvider.Current.GetString(LocalLocalizationResourceNames.SourceLineCountLabel); }
        }

        #endregion

        #region Methods: Event Handler

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
