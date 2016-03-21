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
using System.ComponentModel.Design;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

#endregion

namespace VSEssentials.InsertGuidCommand
{
    internal sealed class InsertGuidCommand
    {
        #region Nested Singleton Class

        private sealed class Singleton
        {
            public static readonly InsertGuidCommand Instance = new InsertGuidCommand();
            static Singleton() { }
        }

        #endregion

        #region Fields

        private readonly CommandID _commandID;
        private readonly OleMenuCommand _menuCommand;
        private Boolean _visible;

        #endregion

        #region Constructors

        private InsertGuidCommand()
        {
            // Instance initialization
            _commandID = new CommandID(new Guid("3020eb2e-3b3d-4e0c-b165-5b8bd559a3cb"), 0x0100);
            _menuCommand = new OleMenuCommand(Execute, null, BeforeQueryStatus, _commandID);
            _visible = true;
        }

        #endregion

        #region Properties

        public CommandID CommandID {
            get { return _commandID; }
        }

        public Boolean IsVisible {
            get { return _visible; }
            set { _visible = value; }
        }

        public MenuCommand MenuCommand {
            get { return _menuCommand; }
        }

        #endregion

        #region Properties: Static

        public static InsertGuidCommand Instance {
            get { return Singleton.Instance; }
        }

        #endregion

        #region Methods: Implementation

        private void BeforeQueryStatus(Object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand != null) {
                menuCommand.Visible = IsVisible;
            }
        }

        private void Execute(Object sender, EventArgs e)
        {
            var textView = GetCurrentTextView();
            if (textView != null) {
                if (textView.HasAggregateFocus) {
                    var caretPosition = GetCurrentCaretPosition(textView);
                    textView.TextBuffer.Insert(caretPosition.BufferPosition, Guid.NewGuid().ToString());
                }
            }
        }

        private CaretPosition GetCurrentCaretPosition(IWpfTextView textView)
        {
            return textView.Caret.Position;
        }

        private IWpfTextView GetCurrentTextView()
        {
            var componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            if (componentModel != null) {
                var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
                if (editorAdapter != null) {
                    var textManager = ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
                    if (textManager != null) {
                        IVsTextView currentView = null;
                        textManager.GetActiveView(1, null, out currentView);
                        if (currentView != null) {
                            return editorAdapter.GetWpfTextView(currentView);
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
