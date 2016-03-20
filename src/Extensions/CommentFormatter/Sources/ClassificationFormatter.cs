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

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Collections.Generic;

#endregion

namespace VSEssentials.CommentFormatter
{
    static class ClassificationFormatter
    {
        #region Methods

        internal static void Fade(IClassificationFormatMap formatMap, IClassificationType classificationType)
        {
            // Argument validation
            if (formatMap == null) {
                throw new ArgumentNullException(nameof(formatMap));
            }
            if (classificationType == null) {
                throw new ArgumentNullException(nameof(classificationType));
            }

            // Perform formatting
            TextFormattingRunProperties properties = formatMap.GetTextProperties(classificationType);
            properties = properties.SetForegroundOpacity(2d / 3d);
            formatMap.SetTextProperties(classificationType, properties);
        }

        internal static void Italicize(IClassificationFormatMap formatMap, IClassificationType classificationType)
        {
            // Argument validation
            if (formatMap == null) {
                throw new ArgumentNullException(nameof(formatMap));
            }
            if (classificationType == null) {
                throw new ArgumentNullException(nameof(classificationType));
            }

            // Perform formatting
            TextFormattingRunProperties properties = formatMap.GetTextProperties(classificationType);

            if (properties.ItalicEmpty || !properties.Italic) {
                properties = properties.SetItalic(true);
                formatMap.SetTextProperties(classificationType, properties);
            }
        }

        internal static void Normalize(IClassificationFormatMap formatMap, IClassificationType classificationType)
        {
            // Argument validation
            if (formatMap == null) {
                throw new ArgumentNullException(nameof(formatMap));
            }
            if (classificationType == null) {
                throw new ArgumentNullException(nameof(classificationType));
            }

            // Perform formatting
            TextFormattingRunProperties properties = formatMap.GetTextProperties(classificationType);

            if (!properties.ItalicEmpty || properties.Italic) {
                properties = properties.SetItalic(false);
                formatMap.SetTextProperties(classificationType, properties);
            }
        }

        internal static void Sharpen(IClassificationFormatMap formatMap, IClassificationType classificationType)
        {
            // Argument validation
            if (formatMap == null) {
                throw new ArgumentNullException(nameof(formatMap));
            }
            if (classificationType == null) {
                throw new ArgumentNullException(nameof(classificationType));
            }

            // Perform formatting
            TextFormattingRunProperties properties = formatMap.GetTextProperties(classificationType);
            properties = properties.SetForegroundOpacity(1d);
            formatMap.SetTextProperties(classificationType, properties);
        }

        #endregion
    }
}
