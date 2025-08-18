// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using System;

namespace DCSystemXml
{
    internal enum ExceptionType
    {
        ArgumentException,
        XmlException,
    }
    public partial class XmlConvert
    {

        public static string ToString(int value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(float value)
        {
            if (float.IsNegativeInfinity(value)) return "-INF";
            if (float.IsPositiveInfinity(value)) return "INF";
            if (IsNegativeZero((double)value))
            {
                return ("-0");
            }
            return value.ToString("R", NumberFormatInfo.InvariantInfo);
        }

        public static string ToString(double value)
        {
            if (double.IsNegativeInfinity(value)) return "-INF";
            if (double.IsPositiveInfinity(value)) return "INF";
            if (IsNegativeZero(value))
            {
                return ("-0");
            }

            return value.ToString("R", NumberFormatInfo.InvariantInfo);
        }

        public static bool ToBoolean(string s)
        {
            s = TrimString(s);
            if (s == "1" || s == "true") return true;
            if (s == "0" || s == "false") return false;
            throw new FormatException("BadFormat:" + s +  " Boolean");
        }


        public static int ToInt32(string s)
        {
            return int.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
        }
        private static volatile string[] s_allDateTimeFormats;

        // NOTE: Do not use this property for reference comparison. It may not be unique.
        private static string[] AllDateTimeFormats
        {
            get
            {
                if (s_allDateTimeFormats == null)
                {
                    CreateAllDateTimeFormats();
                }

                return s_allDateTimeFormats!;
            }
        }

        private static void CreateAllDateTimeFormats()
        {
            if (s_allDateTimeFormats == null)
            {
                // no locking; the array is immutable so it's not a problem that it may get initialized more than once
                s_allDateTimeFormats = new string[] {
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzzzzz", //dateTime
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
                    "HH:mm:ss.FFFFFFF",                  //time
                    "HH:mm:ss.FFFFFFFZ",
                    "HH:mm:ss.FFFFFFFzzzzzz",
                    "yyyy-MM-dd",                   // date
                    "yyyy-MM-ddZ",
                    "yyyy-MM-ddzzzzzz",
                    "yyyy-MM",                      // yearMonth
                    "yyyy-MMZ",
                    "yyyy-MMzzzzzz",
                    "yyyy",                         // year
                    "yyyyZ",
                    "yyyyzzzzzz",
                    "--MM-dd",                      // monthDay
                    "--MM-ddZ",
                    "--MM-ddzzzzzz",
                    "---dd",                        // day
                    "---ddZ",
                    "---ddzzzzzz",
                    "--MM--",                       // month
                    "--MM--Z",
                    "--MM--zzzzzz",
                };
            }
        }

        public delegate DateTime ParseDateTimeStringHandler(string strValue);
        public static ParseDateTimeStringHandler EventParseDateTime = null;

        public static DateTime ToDateTime(string s)
        {
            if (EventParseDateTime != null)
            {
                return EventParseDateTime(s);
            }
            else
            {
                return ToDateTime(s, AllDateTimeFormats);
            }
        }
 

        public static DateTime ToDateTime(string s, string[] formats)
        {
            return DateTime.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
        }

        internal static readonly char[] WhitespaceChars = new char[] { ' ', '\t', '\n', '\r' };

        // Trim a string using XML whitespace characters
        internal static string TrimString(string value)
        {
            return value.Trim(WhitespaceChars);
        }


        internal static bool IsNegativeZero(double value)
        {
            // Simple equals function will report that -0 is equal to +0, so compare bits instead
            if (value == 0 && BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(-0e0))
            {
                return true;
            }
            return false;
        }


        internal static Exception CreateException(string res, string arg, ExceptionType exceptionType, int lineNo, int linePos)
        {
            switch (exceptionType)
            {
                case ExceptionType.ArgumentException:
                    return new ArgumentException(string.Format(res, arg));
                case ExceptionType.XmlException:
                default:
                    return new XmlException(res, arg, lineNo, linePos);
            }
        }


        internal static Exception CreateException(string res, string[] args, ExceptionType exceptionType, int lineNo, int linePos)
        {
            switch (exceptionType)
            {
                case ExceptionType.ArgumentException:
                    return new ArgumentException(string.Format(res, args));
                case ExceptionType.XmlException:
                default:
                    return new XmlException(res, args, lineNo, linePos);
            }
        }

        internal static Exception CreateInvalidSurrogatePairException(char low, char hi)
        {
            return CreateInvalidSurrogatePairException(low, hi, ExceptionType.ArgumentException);
        }

        internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType)
        {
            return CreateInvalidSurrogatePairException(low, hi, exceptionType, 0, 0);
        }

        internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType, int lineNo, int linePos)
        {
            string[] args = new string[] {
                ((uint)hi).ToString("X", CultureInfo.InvariantCulture),
                ((uint)low).ToString("X", CultureInfo.InvariantCulture)
            };
            return CreateException("", args, exceptionType, lineNo, linePos);
        }

        internal static Exception CreateInvalidHighSurrogateCharException(char hi)
        {
            return CreateInvalidHighSurrogateCharException(hi, ExceptionType.ArgumentException);
        }

        internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType)
        {
            return CreateInvalidHighSurrogateCharException(hi, exceptionType, 0, 0);
        }

        internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType, int lineNo, int linePos)
        {
            return CreateException("", ((uint)hi).ToString("X", CultureInfo.InvariantCulture), exceptionType, lineNo, linePos);
        }

    }
}
