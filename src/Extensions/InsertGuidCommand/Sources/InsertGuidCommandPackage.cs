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
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

#endregion

namespace VSEssentials.InsertGuidCommand
{
    [InstalledProductRegistration("#110", "#112", ProductVersion, IconResourceID = 400)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(InsertGuidCommandOptionPage), InsertGuidCommandOptionPage.CategoryName, InsertGuidCommandOptionPage.PageName, 120, 130, false)]
    [Guid(InsertGuidCommandGuids.Package)]
    internal sealed class InsertGuidCommandPackage : Package
    {
        #region Constants

        public const String ProductVersion = "2017.1.1";

        #endregion

        #region Fields

        private OleMenuCommandService _commandService;
        private WindowEvents _windowEvents;

        #endregion

        #region Methods: Event Handler

        private void WindowEvents_WindowClosing(Window Window)
        {
            InsertLastGuidCommand.Instance.IsVisible = false;
            InsertNewGuidCommand.Instance.IsVisible = false;
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            InsertNewGuidCommand.Instance.IsVisible = GotFocus != null && GotFocus.Kind == "Document";
            InsertLastGuidCommand.Instance.IsVisible = InsertNewGuidCommand.Instance.IsVisible;
        }

        #endregion

        #region Methods: Overridden

        protected override void Dispose(Boolean disposing)
        {
            if (disposing) {
                if (_windowEvents != null) {
                    _windowEvents.WindowClosing -= WindowEvents_WindowClosing;
                    _windowEvents.WindowActivated -= WindowEvents_WindowActivated;
                }
                if (_commandService != null) {
                    _commandService.RemoveCommand(InsertLastGuidCommand.Instance.MenuCommand);
                    _commandService.RemoveCommand(InsertNewGuidCommand.Instance.MenuCommand);
                }
            }

            base.Dispose(disposing);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (_commandService != null) {
                _commandService.AddCommand(InsertNewGuidCommand.Instance.MenuCommand);
                _commandService.AddCommand(InsertLastGuidCommand.Instance.MenuCommand);
            }

            if (GetService(typeof(DTE)) is DTE dte) {
                _windowEvents = dte.Events.WindowEvents;
                if (_windowEvents != null) {
                    _windowEvents.WindowActivated += WindowEvents_WindowActivated;
                    _windowEvents.WindowClosing += WindowEvents_WindowClosing;
                }
            }
        }

        #endregion

        #region Methods: Implementation

        internal static void InitializeCommandStates()
        {
            InsertNewGuidCommand.Instance.IsVisible = true;
            InsertLastGuidCommand.Instance.IsVisible = true;
        }

        internal static ITextView GetCurrentTextView()
        {
            if (GetGlobalService(typeof(SComponentModel)) is IComponentModel componentModel) {
                var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
                if (editorAdapter != null) {
                    if (ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager)) is IVsTextManager textManager) {
                        textManager.GetActiveView(1, null, out var currentView);
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
