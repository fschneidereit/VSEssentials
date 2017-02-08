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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

#endregion

namespace VSEssentials.SemanticFormatter
{
    [InstalledProductRegistration("#110", "#112", ProductVersion, IconResourceID = 400)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(SemanticFormatterGuids.Package)]
    //[ProvideOptionPage(typeof(SemanticFormatterOptionPage), "VSEssentials", "Semantic Formatter", 120, 110, false)]
    internal sealed class SemanticFormatterPackage : Package
    {
        #region Constants

        public const String ProductVersion = "2017.0.1";

        #endregion

        #region Constructors

        public SemanticFormatterPackage()
        {
        }

        #endregion

        #region Methods: Overridden

        protected override void Initialize()
        {
            base.Initialize();
        }

        #endregion
    }
}
