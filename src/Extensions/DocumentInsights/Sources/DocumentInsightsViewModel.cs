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

using System;
using System.ComponentModel;

#endregion

namespace Flatcode.VSEssentials.Extensions.DocumentInsights
{
    sealed class DocumentInsightsViewModel : INotifyPropertyChanged
    {
        #region Fields

        readonly LocalLocalizationProvider localizationProvider;
        readonly Int32 sourceCharCount;
        readonly Int32 sourceLineCount;
        Int32 actualCharCount;
        Int32 actualLineCount;
        Boolean showCharInfo;
        Boolean showLineInfo;

        #endregion

        #region Constructors

        public DocumentInsightsViewModel(Int32 lineCount = 0, Int32 charCount = 0)
        {
            // Instance initialization
            localizationProvider = new LocalLocalizationProvider();
            actualLineCount = sourceLineCount = lineCount;
            actualCharCount = sourceCharCount = charCount;

            // Initialize defaults
            showCharInfo = true;
            showLineInfo = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Int32 ActualCharCount {
            get { return actualCharCount; }
            set {
                if (actualCharCount != value) {
                    actualCharCount = value;
                    OnPropertyChanged(nameof(ActualCharCount));
                    OnPropertyChanged(nameof(CharDifference));
                }
            }
        }

        public String ActualCharCountLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.ActualCharCountLabel); }
        }

        public Int32 ActualLineCount {
            get { return actualLineCount; }
            set {
                if (actualLineCount != value) {
                    actualLineCount = value;
                    OnPropertyChanged(nameof(ActualLineCount));
                    OnPropertyChanged(nameof(LineDifference));
                }
            }
        }

        public String ActualLineCountLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.ActualLineCountLabel); }
        }

        public Int32 CharDifference {
            get { return ActualCharCount - SourceCharCount; }
        }

        public String CharInfoLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.CharInfoLabel); }
        }

        public String DifferenceLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.DifferenceLabel); }
        }

        public Int32 LineDifference {
            get { return ActualLineCount - SourceLineCount; }
        }

        public String LineInfoLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.LineInfoLabel); }
        }

        public Boolean ShowCharInfo {
            get { return showCharInfo; }
            set {
                if (showCharInfo != value) {
                    showCharInfo = value;
                    OnPropertyChanged(nameof(ShowCharInfo));
                }
            }
        }

        public Boolean ShowLineInfo {
            get { return showLineInfo; }
            set {
                if (showLineInfo != value) {
                    showLineInfo = value;
                    OnPropertyChanged(nameof(ShowLineInfo));
                }
            }
        }

        public Int32 SourceCharCount {
            get { return sourceCharCount; }
        }

        public String SourceCharCountLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.SourceCharCountLabel); }
        }

        public Int32 SourceLineCount {
            get { return sourceLineCount; }
        }

        public String SourceLineCountLabel {
            get { return localizationProvider.GetString(LocalLocalizationResourceNames.SourceLineCountLabel); }
        }

        #endregion

        #region Methods: Event Handler

        void OnPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
