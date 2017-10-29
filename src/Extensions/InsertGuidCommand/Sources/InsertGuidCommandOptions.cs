/***************************************************************************************************
 *
 *  Copyright © 2017 Florian Schneidereit
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

namespace VSEssentials.InsertGuidCommand
{
    internal sealed class InsertGuidCommandOptions : INotifyPropertyChanged
    {
        #region Fields

        private static readonly Lazy<InsertGuidCommandOptions> _instance =
            new Lazy<InsertGuidCommandOptions>(CreateInstance);

        private GuidLetterCase _guidLetterCase;
        private GuidParenthesis _guidParenthesis;

        #endregion

        #region Constructors

        private InsertGuidCommandOptions()
        {
            // Instance initialization
            _guidLetterCase = GuidLetterCase.Lower;
            _guidParenthesis = GuidParenthesis.None;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public GuidLetterCase GuidLetterCase {
            get { return _guidLetterCase; }
            set {
                if (_guidLetterCase != value) {
                    _guidLetterCase = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(GuidLetterCase)));
                }
            }
        }

        public GuidParenthesis GuidParenthesis {
            get { return _guidParenthesis; }
            set {
                if (_guidParenthesis != value) {
                    _guidParenthesis = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(GuidParenthesis)));
                }
            }
        }

        public static InsertGuidCommandOptions Instance {
            get { return _instance.Value; }
        }

        #endregion

        #region Methods

        private static InsertGuidCommandOptions CreateInstance()
        {
            return new InsertGuidCommandOptions();
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
