/***************************************************************************************************
 *
 *  Copyright © 2018 Florian Schneidereit
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
using System.Globalization;

#endregion

namespace VSEssentials.CommentFormatter
{
    public sealed class BooleanOptionConverter : BooleanConverter
    {
        #region Constructors

        public BooleanOptionConverter()
        {
        }

        #endregion

        #region Methods

        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String)) {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String)) {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            var localizationProvider = CommentFormatterLocalizationProvider.Instance;

            if (value is String s) {
                if (s.Equals(localizationProvider.GetString(CommentFormatterLocalizedResourceNames.YesString), StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
                if (s.Equals(localizationProvider.GetString(CommentFormatterLocalizedResourceNames.NoString), StringComparison.OrdinalIgnoreCase)) {
                    return false;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            var localizationProvider = CommentFormatterLocalizationProvider.Instance;

            if (destinationType == typeof(String)) {
                if (value is Boolean b) {
                    return b ?
                        localizationProvider.GetString(CommentFormatterLocalizedResourceNames.YesString) :
                        localizationProvider.GetString(CommentFormatterLocalizedResourceNames.NoString);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}
