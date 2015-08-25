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

using System;
using System.ComponentModel;

#endregion

namespace Flatcode.VSEssentials.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event)]
    public sealed class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        #region Fields

        Type localizationProvider;
        String resourceName;

        #endregion

        #region Constructors

        public LocalizableDescriptionAttribute() : this(String.Empty)
        {
        }

        public LocalizableDescriptionAttribute(String description) : base(description)
        {
            // Default instance initialization
            localizationProvider = null;
            resourceName = null;
        }

        #endregion

        #region Properties

        public Type LocalizationProvider {
            get { return localizationProvider; }
            set { localizationProvider = value; }
        }

        public String ResourceName {
            get { return resourceName; }
            set { resourceName = value; }
        }

        #endregion

        #region Properties: Overridden

        public override String Description {
            get {
                if (localizationProvider != null) {
                    ILocalizationProvider instance = Activator.CreateInstance(localizationProvider) as ILocalizationProvider;
                    if (instance != null) {
                        String description;
                        if (instance.TryGetString(resourceName, out description)) {
                            return description;
                        }
                    }
                }

                return base.Description;
            }
        }

        #endregion
    }
}
