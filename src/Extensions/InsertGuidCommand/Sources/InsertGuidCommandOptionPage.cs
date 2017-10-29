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
using Microsoft.VisualStudio.Shell;
using VSEssentials.Common;

#endregion

namespace VSEssentials.InsertGuidCommand
{
    public sealed class InsertGuidCommandOptionPage : DialogPage
    {
        #region Constants

        public const String CategoryName = "Extensions";
        public const String PageName = @"VSEssentials\Insert GUID Command";

        #endregion

        #region Properties

        [Category("Format")]
        [LocalizableDisplayName(
            LocalizationProvider = typeof(InsertGuidCommandLocalizationProvider),
            ResourceName = InsertGuidCommandLocalizedResourceNames.GuidLetterCaseOptionDisplayName)]
        [LocalizableDescription(
            LocalizationProvider = typeof(InsertGuidCommandLocalizationProvider),
            ResourceName = InsertGuidCommandLocalizedResourceNames.GuidLetterCaseOptionDescription)]
        [DefaultValue(GuidLetterCase.Lower)]
        [TypeConverter(typeof(EnumOptionConverter))]
        public GuidLetterCase GuidLetterCase {
            get { return InsertGuidCommandOptions.Instance.GuidLetterCase; }
            set { InsertGuidCommandOptions.Instance.GuidLetterCase = value; }
        }

        [Category("Format")]
        [LocalizableDisplayName(
            LocalizationProvider = typeof(InsertGuidCommandLocalizationProvider),
            ResourceName = InsertGuidCommandLocalizedResourceNames.GuidParenthesisOptionDisplayName)]
        [LocalizableDescription(
            LocalizationProvider = typeof(InsertGuidCommandLocalizationProvider),
            ResourceName = InsertGuidCommandLocalizedResourceNames.GuidParenthesisOptionDescription)]
        [DefaultValue(GuidParenthesis.None)]
        [TypeConverter(typeof(EnumOptionConverter))]
        public GuidParenthesis GuidParenthesis {
            get { return InsertGuidCommandOptions.Instance.GuidParenthesis; }
            set { InsertGuidCommandOptions.Instance.GuidParenthesis = value; }
        }

        #endregion
    }
}
