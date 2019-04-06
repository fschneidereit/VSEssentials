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
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace VSEssentials.SemanticFormatter
{
    internal sealed class OutputLogger
    {
        #region Nested Singleton Class

        private sealed class Singleton
        {
            public static readonly OutputLogger Instance = new OutputLogger();
            static Singleton() { }
        }

        #endregion

        #region Constants

        public const String PaneGuid = "734485c3-1e43-438c-bd63-d3befb8c2434";
        public const String PaneTitle = "Output Logger";

        #endregion

        #region Fields

        private readonly IVsOutputWindowPane _pane;

        #endregion

        #region Constructors

        private OutputLogger()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow != null) {
                Guid paneGuid = new Guid(PaneGuid);
                Int32 result = outputWindow.CreatePane(ref paneGuid, PaneTitle, 1, 0);
                if (result != 0) {
                    Marshal.ThrowExceptionForHR(result);
                }
                outputWindow.GetPane(ref paneGuid, out _pane);
                if (_pane != null) {
                    _pane.Activate();
                }
            }
        }

        #endregion

        #region Properties: Static

        public static OutputLogger Current {
            get { return Singleton.Instance; }
        }

        #endregion

        #region Methods

        public void Write(String message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (_pane != null) {
                _pane.OutputString(message);
            }
        }

        public void WriteLine(String message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Write(message + Environment.NewLine);
        }

        #endregion
    }
}
