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
using System.Collections.Generic;

#endregion

namespace VSEssentials.Extensions.CommentFormatter
{
    static class KnownClassificationTypeNames
    {
        #region Constants

        internal const String Comment = "comment";
        internal const String XmlDocCommentAttributeName = "xml doc comment - attribute name";
        internal const String XmlDocCommentAttributeQuotes = "xml doc comment - attribute quotes";
        internal const String XmlDocCommentAttributeValue = "xml doc comment - attribute value";
        internal const String XmlDocCommentCDATASection = "xml doc comment - cdata section";
        internal const String XmlDocCommentComment = "xml doc comment - comment";
        internal const String XmlDocCommentDelimiter = "xml doc comment - delimiter";
        internal const String XmlDocCommentEntityReference = "xml doc comment - entity reference";
        internal const String XmlDocCommentName = "xml doc comment - name";
        internal const String XmlDocCommentProcessingInstruction = "xml doc comment - processing instruction";
        internal const String XmlDocCommentText = "xml doc comment - text";

        #endregion

        #region Fields

        static readonly IList<String> comments = new List<String> {
            Comment,
            ClassificationTypeNames.CommentBlock // Know type within the comment formatter
        };

        static readonly IList<String> documentationComments = new List<String> {
            XmlDocCommentAttributeName,
            XmlDocCommentAttributeQuotes,
            XmlDocCommentAttributeValue,
            XmlDocCommentCDATASection,
            XmlDocCommentComment,
            XmlDocCommentDelimiter,
            XmlDocCommentEntityReference,
            XmlDocCommentName,
            XmlDocCommentProcessingInstruction,
            XmlDocCommentText
        };

        static readonly IList<String> documentationTags = new List<String> {
            XmlDocCommentAttributeName,
            XmlDocCommentAttributeQuotes,
            XmlDocCommentAttributeValue,
            XmlDocCommentCDATASection,
            XmlDocCommentDelimiter,
            XmlDocCommentEntityReference,
            XmlDocCommentName,
            XmlDocCommentProcessingInstruction
        };

        #endregion

        #region Properties

        internal static IList<String> Comments {
            get { return comments; }
        }

        internal static IList<String> DocumentationComments {
            get { return documentationComments; }
        }

        internal static IList<String> DocumentationTags {
            get { return documentationTags; }
        }

        #endregion
    }
}
