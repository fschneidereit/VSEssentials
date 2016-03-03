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
using System.ComponentModel.Composition;

#endregion

namespace VSEssentials.Extensions.CommentFormatter
{
    sealed class CommentFormatterOptions : INotifyPropertyChanged
    {
        #region Singleton Class

        class Singleton
        {
            internal static readonly CommentFormatterOptions Instance = new CommentFormatterOptions();
            static Singleton() { }
        }

        #endregion

        #region Fields

        Boolean fadeDocumentationTags;
        Boolean italicizeComments;
        Boolean italicizeDocumentationComments;

        #endregion

        #region Constructors

        CommentFormatterOptions()
        {
            // Default instance initialization
            fadeDocumentationTags = true;
            italicizeComments = false;
            italicizeDocumentationComments = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Boolean FadeDocumentationTags {
            get { return fadeDocumentationTags; }
            set {
                if (fadeDocumentationTags != value) {
                    fadeDocumentationTags = value;
                    OnPropertyChanged(nameof(FadeDocumentationTags));
                }
            }
        }

        public Boolean ItalicizeComments {
            get { return italicizeComments; }
            set {
                if (italicizeComments != value) {
                    italicizeComments = value;
                    OnPropertyChanged(nameof(ItalicizeComments));
                }
            }
        }

        public Boolean ItalicizeDocumentationComments {
            get { return italicizeDocumentationComments; }
            set {
                if (italicizeDocumentationComments != value) {
                    italicizeDocumentationComments = value;
                    OnPropertyChanged(nameof(ItalicizeDocumentationComments));
                }
            }
        }

        #endregion

        #region Properties: Static

        public static CommentFormatterOptions Current {
            get { return Singleton.Instance; }
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
