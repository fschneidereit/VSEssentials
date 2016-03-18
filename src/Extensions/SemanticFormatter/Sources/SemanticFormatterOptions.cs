/***************************************************************************************************
 *
 *  Copyright © 2016 Florian Schneidereit
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

namespace VSEssentials.SemanticFormatter
{
    internal sealed class SemanticFormatterOptions : INotifyPropertyChanged
    {
        #region Nested Singleton Class

        private sealed class Singleton
        {
            internal static readonly SemanticFormatterOptions Instance = new SemanticFormatterOptions();
            static Singleton() { }
        }

        #endregion

        #region Fields

        private Boolean _enableSemanticFormatting;

        #endregion

        #region Constructors

        public SemanticFormatterOptions()
        {
            // Instance initialization
            _enableSemanticFormatting = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Boolean EnableSemanticFormatting {
            get { return _enableSemanticFormatting; }
            set {
                if (_enableSemanticFormatting != value) {
                    _enableSemanticFormatting = value;
                    OnPropertyChanged(nameof(EnableSemanticFormatting));
                }
            }
        }

        #endregion

        #region Properties: Static

        public static SemanticFormatterOptions Current {
            get { return Singleton.Instance; }
        }

        #endregion

        #region Methods: Event Handler

        private void OnPropertyChanged(String propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
