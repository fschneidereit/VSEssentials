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
        #region Constants

        public const Int32 CommandId = 0x0100;
        public const String CommandSetGuid = "3020eb2e-3b3d-4e0c-b165-5b8bd559a3cb";

        #endregion

        #region Fields

        private readonly Package _owner;

        #endregion

        #region Fields: Static

        private static InsertGuidCommand _instance;

        #endregion

        #region Constructors

        private InsertGuidCommand(Package owner)
        {
            // Argument validation
            if (owner == null) {
                throw new ArgumentNullException(nameof(owner));
            }

            _owner = owner;

            OleMenuCommandService commandService = ((IServiceProvider)_owner).GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null) {
                var insertGuidCommandID = new CommandID(new Guid(CommandSetGuid), CommandId);
                var insertGuidMenuItem = new OleMenuCommand(ExecuteCallback, insertGuidCommandID);
                insertGuidMenuItem.BeforeQueryStatus += BeforeQueryStatus;
                commandService.AddCommand(insertGuidMenuItem);
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Properties: Static

        public static InsertGuidCommand Instance {
            get { return _instance; }
            private set { _instance = value; }
        }

        #endregion

        #region Methods: Static

        public static void Initialize(Package owner)
        {
            if (_instance == null) {
                _instance = new InsertGuidCommand(owner);
            }
        }

        #endregion

        #region Methods: Implementation

        private void BeforeQueryStatus(Object sender, EventArgs e)
        {
        }

        private void ExecuteCallback(Object sender, EventArgs e)
        {
            IWpfTextView textView = GetCurrentTextView();
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
