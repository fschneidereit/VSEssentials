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
using System.Collections.Concurrent;
using System.Reflection;
using System.Resources;

#endregion

namespace VSEssentials.Common
{
    public abstract class LocalizationProvider : ILocalizationProvider
    {
        #region Constants

        public const String BaseResourceName = "ExtensionResources";

        #endregion

        #region Fields

        readonly ResourceManager resourceManager;

        #endregion

        #region Fields: Static

        static readonly ConcurrentDictionary<String, ResourceManager> resourceManagers;

        #endregion

        #region Constructors

        static LocalizationProvider()
        {
            resourceManagers = new ConcurrentDictionary<String, ResourceManager>();
        }

        protected LocalizationProvider(Assembly assembly)
        {
            // Argument validation
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }

            // Default instance initialization
            resourceManager = resourceManagers.GetOrAdd(assembly.FullName, new ResourceManager(BaseResourceName, assembly));
        }

        #endregion

        #region Properties


        protected ResourceManager ResourceManager {
            get { return resourceManager; }
        }

        #endregion

        #region Methods

        public virtual Boolean TryGetString(String resourceName, out String value)
        {
            // Argument validation
            if (resourceName == null) {
                throw new ArgumentNullException(nameof(resourceName));
            }

            return (value = resourceManager.GetString(resourceName)) != null;
        }

        #endregion
    }
}
