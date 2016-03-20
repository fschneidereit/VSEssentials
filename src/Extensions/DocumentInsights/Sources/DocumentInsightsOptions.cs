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

#endregion

namespace VSEssentials.DocumentInsights
{
    internal sealed class DocumentInsightsOptions : INotifyPropertyChanged
    {
        #region Singleton Class

        private sealed class Singleton
        {
            public static readonly DocumentInsightsOptions Instance = new DocumentInsightsOptions();
            static Singleton() { }
        }

        #endregion

        #region Fields

        Boolean _enableCharInfo;
        Boolean _enableLineInfo;
        Boolean _enableEncodingInfo;
        Boolean _showMargin;

        #endregion

        #region Constructors

        DocumentInsightsOptions()
        {
            // Default instance initialization
            _enableCharInfo = true;
            _enableLineInfo = true;
            _enableEncodingInfo = true;
            _showMargin = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Boolean EnableCharInfo {
            get { return _enableCharInfo; }
            set {
                if (_enableCharInfo != value) {
                    _enableCharInfo = value;
                    OnPropertyChanged(nameof(EnableCharInfo));
                }
            }
        }

        public Boolean EnableEncodingInfo {
            get { return _enableEncodingInfo; }
            set {
                if (_enableEncodingInfo != value) {
                    _enableEncodingInfo = value;
                    OnPropertyChanged(nameof(EnableEncodingInfo));
                }
            }
        }

        public Boolean EnableLineInfo {
            get { return _enableLineInfo; }
            set {
                if (_enableLineInfo != value) {
                    _enableLineInfo = value;
                    OnPropertyChanged(nameof(EnableLineInfo));
                }
            }
        }

        public Boolean ShowMargin {
            get { return _showMargin; }
            set {
                if (_showMargin != value) {
                    _showMargin = value;
                    OnPropertyChanged(nameof(ShowMargin));
                }
            }
        }

        #endregion

        #region Properties: Current

        public static DocumentInsightsOptions Current {
            get { return Singleton.Instance; }
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
