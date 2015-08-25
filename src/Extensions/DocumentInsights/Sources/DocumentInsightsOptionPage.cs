/***************************************************************************************************
 *
 *  Copyright © 2015 Flatcode.net
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

using Flatcode.VSEssentials.Common;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;

#endregion

namespace Flatcode.VSEssentials.Extensions.DocumentInsights
{
    public class DocumentInsightsOptionPage : DialogPage
    {
        #region Properties

        [Category("Appearance")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.EnableCharInfoDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.EnableCharInfoDescription)]
        [DefaultValue(true)]
        public Boolean EnableCharInfo {
            get { return DocumentInsightsOptions.Current.EnableCharInfo; }
            set { DocumentInsightsOptions.Current.EnableCharInfo = value; }
        }

        [Category("Appearance")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.EnableLineInfoDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.EnableLineInfoDescription)]
        [DefaultValue(true)]
        public Boolean EnableLineInfo
        {
            get { return DocumentInsightsOptions.Current.EnableLineInfo; }
            set { DocumentInsightsOptions.Current.EnableLineInfo = value; }
        }

        [Category("Behavior")]
        [LocalizableDisplayName(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ShowMarginDisplayName)]
        [LocalizableDescription(LocalizationProvider = typeof(LocalLocalizationProvider), ResourceName = LocalLocalizationResourceNames.ShowMarginDescription)]
        [DefaultValue(true)]
        public Boolean ShowMargin {
            get { return DocumentInsightsOptions.Current.ShowMargin; }
            set { DocumentInsightsOptions.Current.ShowMargin = value; }
        }

        #endregion
    }
}
