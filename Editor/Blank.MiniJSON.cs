﻿#if UNITY_IOS
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Globalization;


/* Based on the JSON parser from
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * I simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 */
/// <summary>
/// This class encodes and decodes JSON strings.
/// Spec. details, see http://www.json.org/
/// 
/// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
/// All numbers are parsed to doubles.
/// </summary>

namespace GameFrameX.Xcode.Editor
{
    public sealed class MiniJSON
    {
        private const int TOKEN_NONE = 0;
        private const int TOKEN_CURLY_OPEN = 1;
        private const int TOKEN_CURLY_CLOSE = 2;
        private const int TOKEN_SQUARED_OPEN = 3;
        private const int TOKEN_SQUARED_CLOSE = 4;
        private const int TOKEN_COLON = 5;
        private const int TOKEN_COMMA = 6;
        private const int TOKEN_STRING = 7;
        private const int TOKEN_NUMBER = 8;
        private const int TOKEN_TRUE = 9;
        private const int TOKEN_FALSE = 10;
        private const int TOKEN_NULL = 11;
        private const int BUILDER_CAPACITY = 2000;

        /// <summary>
        /// On decoding, this value holds the position at which the parse failed (-1 = no error).
        /// </summary>
        private static int _lastErrorIndex = -1;

        private static string _lastDecode = "";


        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
        public static object JsonDecode(string json)
        {
            // save the string for debug information
            MiniJSON._lastDecode = json;

            if (json != null)
            {
                char[] charArray = json.ToCharArray();
                int index = 0;
                bool success = true;
                object value = MiniJSON.ParseValue(charArray, ref index, ref success);

                if (success)
                    MiniJSON._lastErrorIndex = -1;
                else
                    MiniJSON._lastErrorIndex = index;

                return value;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Converts a Hashtable / ArrayList / Dictionary(string,string) object into a JSON string
        /// </summary>
        /// <param name="json">A Hashtable / ArrayList</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string JsonEncode(object json)
        {
            var builder = new StringBuilder(BUILDER_CAPACITY);
            var success = MiniJSON.SerializeValue(json, builder);

            return (success ? builder.ToString() : null);
        }


        /// <summary>
        /// On decoding, this function returns the position at which the parse failed (-1 = no error).
        /// </summary>
        /// <returns></returns>
        public static bool LastDecodeSuccessful()
        {
            return (MiniJSON._lastErrorIndex == -1);
        }


        /// <summary>
        /// On decoding, this function returns the position at which the parse failed (-1 = no error).
        /// </summary>
        /// <returns></returns>
        public static int GetLastErrorIndex()
        {
            return MiniJSON._lastErrorIndex;
        }


        /// <summary>
        /// If a decoding error occurred, this function returns a piece of the JSON string 
        /// at which the error took place. To ease debugging.
        /// </summary>
        /// <returns></returns>
        public static string GetLastErrorSnippet()
        {
            if (MiniJSON._lastErrorIndex == -1)
            {
                return "";
            }
            else
            {
                int startIndex = MiniJSON._lastErrorIndex - 5;
                int endIndex = MiniJSON._lastErrorIndex + 15;
                if (startIndex < 0)
                    startIndex = 0;

                if (endIndex >= MiniJSON._lastDecode.Length)
                    endIndex = MiniJSON._lastDecode.Length - 1;

                return MiniJSON._lastDecode.Substring(startIndex, endIndex - startIndex + 1);
            }
        }


        #region Parsing

        private static Hashtable ParseObject(char[] json, ref int index)
        {
            Hashtable table = new Hashtable();
            int token;

            // {
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                token = LookAhead(json, index);
                if (token == MiniJSON.TOKEN_NONE)
                {
                    return null;
                }
                else if (token == MiniJSON.TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == MiniJSON.TOKEN_CURLY_CLOSE)
                {
                    NextToken(json, ref index);
                    return table;
                }
                else
                {
                    // name
                    string name = ParseString(json, ref index);
                    if (name == null)
                    {
                        return null;
                    }

                    // :
                    token = NextToken(json, ref index);
                    if (token != MiniJSON.TOKEN_COLON)
                        return null;

                    // value
                    bool success = true;
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                        return null;

                    table[name] = value;
                }
            }

            return table;
        }


        private static ArrayList ParseArray(char[] json, ref int index)
        {
            ArrayList array = new ArrayList();

            // [
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                int token = LookAhead(json, index);
                if (token == MiniJSON.TOKEN_NONE)
                {
                    return null;
                }
                else if (token == MiniJSON.TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == MiniJSON.TOKEN_SQUARED_CLOSE)
                {
                    NextToken(json, ref index);
                    break;
                }
                else
                {
                    bool success = true;
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                        return null;

                    array.Add(value);
                }
            }

            return array;
        }


        private static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (LookAhead(json, index))
            {
                case MiniJSON.TOKEN_STRING:
                    return ParseString(json, ref index);
                case MiniJSON.TOKEN_NUMBER:
                    return ParseNumber(json, ref index);
                case MiniJSON.TOKEN_CURLY_OPEN:
                    return ParseObject(json, ref index);
                case MiniJSON.TOKEN_SQUARED_OPEN:
                    return ParseArray(json, ref index);
                case MiniJSON.TOKEN_TRUE:
                    NextToken(json, ref index);
                    return Boolean.Parse("TRUE");
                case MiniJSON.TOKEN_FALSE:
                    NextToken(json, ref index);
                    return Boolean.Parse("FALSE");
                case MiniJSON.TOKEN_NULL:
                    NextToken(json, ref index);
                    return null;
                case MiniJSON.TOKEN_NONE:
                    break;
            }

            success = false;
            return null;
        }


        private static string ParseString(char[] json, ref int index)
        {
            string s = "";
            char c;

            EatWhitespace(json, ref index);

            // "
            c = json[index++];

            bool complete = false;
            while (!complete)
            {
                if (index == json.Length)
                    break;

                c = json[index++];
                if (c == '"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == json.Length)
                        break;

                    c = json[index++];
                    if (c == '"')
                    {
                        s += '"';
                    }
                    else if (c == '\\')
                    {
                        s += '\\';
                    }
                    else if (c == '/')
                    {
                        s += '/';
                    }
                    else if (c == 'b')
                    {
                        s += '\b';
                    }
                    else if (c == 'f')
                    {
                        s += '\f';
                    }
                    else if (c == 'n')
                    {
                        s += '\n';
                    }
                    else if (c == 'r')
                    {
                        s += '\r';
                    }
                    else if (c == 't')
                    {
                        s += '\t';
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            char[] unicodeCharArray = new char[4];
                            Array.Copy(json, index, unicodeCharArray, 0, 4);

                            uint codePoint = UInt32.Parse(new string(unicodeCharArray), System.Globalization.NumberStyles.HexNumber);

                            // convert the integer codepoint to a unicode char and add to string
                            s += Char.ConvertFromUtf32((int)codePoint);

                            // skip 4 chars
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    s += c;
                }
            }

            if (!complete)
                return null;

            return s;
        }


        private static double ParseNumber(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);

            int lastIndex = GetLastIndexOfNumber(json, index);
            int charLength = (lastIndex - index) + 1;
            char[] numberCharArray = new char[charLength];

            Array.Copy(json, index, numberCharArray, 0, charLength);
            index = lastIndex + 1;
            return Double.Parse(new string(numberCharArray)); // , CultureInfo.InvariantCulture);
        }


        private static int GetLastIndexOfNumber(char[] json, int index)
        {
            int lastIndex;
            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }

            return lastIndex - 1;
        }


        private static void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    break;
                }
        }


        private static int LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return NextToken(json, ref saveIndex);
        }


        private static int NextToken(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);

            if (index == json.Length)
            {
                return MiniJSON.TOKEN_NONE;
            }

            char c = json[index];
            index++;
            switch (c)
            {
                case '{':
                    return MiniJSON.TOKEN_CURLY_OPEN;
                case '}':
                    return MiniJSON.TOKEN_CURLY_CLOSE;
                case '[':
                    return MiniJSON.TOKEN_SQUARED_OPEN;
                case ']':
                    return MiniJSON.TOKEN_SQUARED_CLOSE;
                case ',':
                    return MiniJSON.TOKEN_COMMA;
                case '"':
                    return MiniJSON.TOKEN_STRING;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                    return MiniJSON.TOKEN_NUMBER;
                case ':':
                    return MiniJSON.TOKEN_COLON;
            }

            index--;

            int remainingLength = json.Length - index;

            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' &&
                    json[index + 1] == 'a' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 's' &&
                    json[index + 4] == 'e')
                {
                    index += 5;
                    return MiniJSON.TOKEN_FALSE;
                }
            }

            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' &&
                    json[index + 1] == 'r' &&
                    json[index + 2] == 'u' &&
                    json[index + 3] == 'e')
                {
                    index += 4;
                    return MiniJSON.TOKEN_TRUE;
                }
            }

            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' &&
                    json[index + 1] == 'u' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 'l')
                {
                    index += 4;
                    return MiniJSON.TOKEN_NULL;
                }
            }

            return MiniJSON.TOKEN_NONE;
        }

        #endregion


        #region Serialization

        private static bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder)
        {
            if (objectOrArray is Hashtable)
            {
                return SerializeObject((Hashtable)objectOrArray, builder);
            }
            else if (objectOrArray is ArrayList)
            {
                return SerializeArray((ArrayList)objectOrArray, builder);
            }
            else
            {
                return false;
            }
        }


        private static bool SerializeObject(Hashtable anObject, StringBuilder builder)
        {
            builder.Append("{");

            IDictionaryEnumerator e = anObject.GetEnumerator();
            bool first = true;
            while (e.MoveNext())
            {
                string key = e.Key.ToString();
                object value = e.Value;

                if (!first)
                {
                    builder.Append(", ");
                }

                SerializeString(key, builder);
                builder.Append(":");
                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("}");
            return true;
        }


        private static bool SerializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
        {
            builder.Append("{");

            bool first = true;
            foreach (var kv in dict)
            {
                if (!first)
                    builder.Append(", ");

                SerializeString(kv.Key, builder);
                builder.Append(":");
                SerializeString(kv.Value, builder);

                first = false;
            }

            builder.Append("}");
            return true;
        }


        private static bool SerializeArray(ArrayList anArray, StringBuilder builder)
        {
            builder.Append("[");

            bool first = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object value = anArray[i];

                if (!first)
                {
                    builder.Append(", ");
                }

                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("]");
            return true;
        }


        private static bool SerializeValue(object value, StringBuilder builder)
        {
            //Type t = value.GetType();
            //UnityEngine.Debug.Log("type: " + t.ToString() + " isArray: " + t.IsArray);

            if (value == null)
            {
                builder.Append("null");
            }
            else if (value.GetType().IsArray)
            {
                SerializeArray(new ArrayList((ICollection)value), builder);
            }
            else if (value is string)
            {
                SerializeString((string)value, builder);
            }
            else if (value is Char)
            {
                SerializeString(Convert.ToString((char)value), builder);
            }
            else if (value is decimal)
            {
                SerializeString(Convert.ToString((decimal)value), builder);
            }
            else if (value is Hashtable)
            {
                SerializeObject((Hashtable)value, builder);
            }
            else if (value is Dictionary<string, string>)
            {
                SerializeDictionary((Dictionary<string, string>)value, builder);
            }
            else if (value is ArrayList)
            {
                SerializeArray((ArrayList)value, builder);
            }
            else if ((value is Boolean) && ((Boolean)value == true))
            {
                builder.Append("true");
            }
            else if ((value is Boolean) && ((Boolean)value == false))
            {
                builder.Append("false");
            }
            else if (value.GetType().IsPrimitive)
            {
                SerializeNumber(Convert.ToDouble(value), builder);
            }
            else
            {
                return false;
            }

            return true;
        }


        private static void SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");

            char[] charArray = aString.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (c == '"')
                {
                    builder.Append("\\\"");
                }
                else if (c == '\\')
                {
                    builder.Append("\\\\");
                }
                else if (c == '\b')
                {
                    builder.Append("\\b");
                }
                else if (c == '\f')
                {
                    builder.Append("\\f");
                }
                else if (c == '\n')
                {
                    builder.Append("\\n");
                }
                else if (c == '\r')
                {
                    builder.Append("\\r");
                }
                else if (c == '\t')
                {
                    builder.Append("\\t");
                }
                else
                {
                    int codepoint = Convert.ToInt32(c);
                    if ((codepoint >= 32) && (codepoint <= 126))
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                    }
                }
            }

            builder.Append("\"");
        }


        private static void SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture)); // , CultureInfo.InvariantCulture));
        }

        #endregion
    }


    #region Extension methods

    public static class MiniJsonExtensions
    {
        public static string ToJson(this Hashtable obj)
        {
            return MiniJSON.JsonEncode(obj);
        }


        public static string ToJson(this Dictionary<string, string> obj)
        {
            return MiniJSON.JsonEncode(obj);
        }


        public static ArrayList ArrayListFromJson(this string json)
        {
            return MiniJSON.JsonDecode(json) as ArrayList;
        }


        public static Hashtable HashtableFromJson(this string json)
        {
            return MiniJSON.JsonDecode(json) as Hashtable;
        }
    }

    #endregion
}
#endif