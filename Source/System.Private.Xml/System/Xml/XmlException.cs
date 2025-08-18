// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

namespace DCSystemXml
{
    /// <devdoc>
    ///    <para>Returns detailed information about the last parse error, including the error
    ///       number, line number, character position, and a text description.</para>
    /// </devdoc>
    public class XmlException : Exception
    {
        private readonly string _res;
        private readonly string[] _args; // this field is not used, it's here just V1.1 serialization compatibility
        private readonly int _lineNumber;
        private readonly int _linePosition;

        public XmlException()
        {
        }


        internal XmlException(string res, string arg, int lineNumber, int linePosition) :
            this(res, new string[] { arg }, null, lineNumber, linePosition, null)
        { }


        internal XmlException(string res, string[] args, int lineNumber, int linePosition) :
            this(res, args, null, lineNumber, linePosition, null)
        { }


        internal XmlException(string res, string[] args, Exception? innerException, int lineNumber, int linePosition, string? sourceUri) :
            base(CreateMessage(res, args, lineNumber, linePosition), innerException)
        {
            //HResult = HResults.Xml;
            _res = res;
            _args = args;
            //_sourceUri = sourceUri;
            _lineNumber = lineNumber;
            _linePosition = linePosition;
        }

        private static string CreateMessage(string res, string[] args, int lineNumber, int linePosition)
        {
            string message = (args == null) ? res : string.Format(res, args);

            // Line information is available -> we need to append it to the error message
            if (lineNumber != 0)
            {
                //string lineNumberStr = lineNumber.ToString(CultureInfo.InvariantCulture);
                //string linePositionStr = linePosition.ToString(CultureInfo.InvariantCulture);

                message = "MessageWithErrorPosition:" + message;
            }
            return message;
        }
    }
}
