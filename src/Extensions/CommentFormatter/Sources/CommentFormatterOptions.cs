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

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

#endregion

namespace VSEssentials.CommentFormatter
{
    internal sealed class CommentFormatterOptions : INotifyPropertyChanged
    {
        #region Fields

        private static readonly Lazy<CommentFormatterOptions> _instance =
            new Lazy<CommentFormatterOptions>(CreateInstance);

        private Boolean _fadeDocumentationTags;
        private Boolean _italicizeComments;
        private Boolean _italicizeDocumentationComments;

        #endregion

        #region Constructors

        private CommentFormatterOptions()
        {
            // Instance initialization
            _fadeDocumentationTags = true;
            _italicizeComments = false;
            _italicizeDocumentationComments = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Boolean FadeDocumentationTags {
            get { return _fadeDocumentationTags; }
            set {
                if (_fadeDocumentationTags != value) {
                    _fadeDocumentationTags = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(FadeDocumentationTags)));
                }
            }
        }

        public static CommentFormatterOptions Instance {
            get { return _instance.Value; }
        }

        public Boolean ItalicizeComments {
            get { return _italicizeComments; }
            set {
                if (_italicizeComments != value) {
                    _italicizeComments = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ItalicizeComments)));
                }
            }
        }

        public Boolean ItalicizeDocumentationComments {
            get { return _italicizeDocumentationComments; }
            set {
                if (_italicizeDocumentationComments != value) {
                    _italicizeDocumentationComments = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ItalicizeDocumentationComments)));
                }
            }
        }

        #endregion

        #region Methods

        private static CommentFormatterOptions CreateInstance()
        {
            return new CommentFormatterOptions();
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
