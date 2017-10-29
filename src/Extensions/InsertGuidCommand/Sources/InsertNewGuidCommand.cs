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
using Microsoft.VisualStudio.Shell;

#endregion

namespace VSEssentials.InsertGuidCommand
{
    internal sealed class InsertNewGuidCommand
    {
        #region Constants

        public const Int32 CommandID = 0x0100;

        #endregion

        #region Fields

        private static readonly Lazy<InsertNewGuidCommand> _instance =
            new Lazy<InsertNewGuidCommand>(CreateInstance);

        private readonly CommandID _commandID;
        private readonly OleMenuCommand _menuCommand;
        private Boolean _visible;

        #endregion

        #region Constructors

        private InsertNewGuidCommand()
        {
            // Instance initialization
            _commandID = new CommandID(new Guid(InsertGuidCommandGuids.MenuGroup), CommandID);
            _menuCommand = new OleMenuCommand(Invoke, null, BeforeQueryStatus, _commandID);
            _visible = false;
        }

        #endregion

        #region Properties

        public static InsertNewGuidCommand Instance {
            get { return _instance.Value; }
        }

        public Boolean IsVisible {
            get { return _visible; }
            set { _visible = value; }
        }

        public MenuCommand MenuCommand {
            get { return _menuCommand; }
        }

        #endregion

        #region Methods

        private void BeforeQueryStatus(Object sender, EventArgs e)
        {
            if (sender is OleMenuCommand menuCommand) {
                menuCommand.Visible = IsVisible;
            }
        }

        private static InsertNewGuidCommand CreateInstance()
        {
            return new InsertNewGuidCommand();
        }

        private void Invoke(Object sender, EventArgs e)
        {
            var guid = Guid.NewGuid();

            // Get current text view
            var textView = InsertGuidCommandPackage.GetCurrentTextView();
            if (textView != null) {
                // Has aggregate focus?
                if (textView.HasAggregateFocus) {
                    // Format GUID
                    var guidString = guid.ToString();
                    if (InsertGuidCommandOptions.Instance.GuidLetterCase == GuidLetterCase.Upper) {
                        guidString = guidString.ToUpper();
                    }
                    switch (InsertGuidCommandOptions.Instance.GuidParenthesis) {
                        case GuidParenthesis.CurlyBrackets:
                            guidString = $"{{{guidString}}}";
                            break;
                        case GuidParenthesis.SquareBrackets:
                            guidString = $"[{guidString}]";
                            break;
                        case GuidParenthesis.AngleBrackets:
                            guidString = $"<{guidString}>";
                            break;
                        case GuidParenthesis.RoundBrackets:
                            guidString = $"({guidString})";
                            break;
                    }
                    // Insert GUID
                    textView.TextBuffer.Insert(textView.Caret.Position.BufferPosition, guidString);
                    // Update last guid command
                    InsertLastGuidCommand.Instance.LastGuid = guid;
                }
            }
        }

        #endregion
    }
}
