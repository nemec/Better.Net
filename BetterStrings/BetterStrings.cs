using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart

namespace BetterStrings
{
    /// <summary>
    /// Adds various string extensions
    /// </summary>
    public static partial class BetterStrings
    {
        #region Builtin string formatting overrides

        /// <summary>
        /// Format the input string with <param name="arg" /> as
        /// its sole parameter.
        /// </summary>
        /// <param name="fmt">Format string.</param>
        /// <param name="arg">Parameter for index 0.</param>
        /// <returns>Formatted string.</returns>
        public static string Format(this string fmt, object arg)
        {
            return String.Format(fmt, arg);
        }

        /// <summary>
        /// Format the input string with two parameters.
        /// </summary>
        /// <param name="fmt">Format string.</param>
        /// <param name="arg0">Parameter for index 0.</param>
        /// <param name="arg1">Parameter for index 1.</param>
        /// <returns>Formatted string.</returns>
        public static string Format(this string fmt, object arg0, object arg1)
        {
            return String.Format(fmt, arg0, arg1);
        }

        /// <summary>
        /// Format the input string with three parameters.
        /// </summary>
        /// <param name="fmt">Format string.</param>
        /// <param name="arg0">Parameter for index 0.</param>
        /// <param name="arg1">Parameter for index 1.</param>
        /// <param name="arg2">Parameter for index 2.</param>
        /// <returns>Formatted string.</returns>
        public static string Format(this string fmt, object arg0, object arg1, object arg2)
        {
            return String.Format(fmt, arg0, arg1, arg2);
        }

        /// <summary>
        /// Format the input string with a variable number
        /// of parameters.
        /// </summary>
        /// <param name="fmt">Format string.</param>
        /// <param name="args">Positional arguments.</param>
        /// <returns>Formatted string.</returns>
        public static string Format(this string fmt, params object[] args)
        {
            return String.Format(fmt, args);
        }

        /// <summary>
        /// Format the input string with a variable number of parameters,
        /// using the given IFormatProvider.
        /// </summary>
        /// <param name="fmt">Format string.</param>
        /// <param name="provider">Format provider.</param>
        /// <param name="args">Positional arguments.</param>
        /// <returns></returns>
        public static string Format(this string fmt, IFormatProvider provider, params object[] args)
        {
            return String.Format(provider, fmt, args);
        }

        #endregion

        #region Dictionary format extensions

        private const string ReservedFormatCharacters = "{},:";
        private const char OpenBrace = '{';

        public static string Format(this string fmt, IDictionary<string, object> dict)
        {
            var builder = new StringBuilder();

            // Conversion table for keys to indexes.
            var order = new Dictionary<string, int>();

            // Place values in the correct index order.
            var values = new object[dict.Count];

            var i = 0;
            foreach (var pair in dict)
            {
                order[pair.Key] = i;
                values[i] = pair.Value;
                i++;
            }

            var key = new StringBuilder();
            var parsingKey = false;

            foreach (var ch in fmt)
            {
                if (!parsingKey)
                {
                    builder.Append(ch);
                    if (ch == OpenBrace)
                    {
                        parsingKey = true;
                        continue;
                    }
                    if (!ReservedFormatCharacters.Contains(fmt))
                    {
                        continue;
                    }
                }
                else if (key.Length == 0 && ch == OpenBrace)
                {
                    parsingKey = false;
                    builder.Append(ch);
                    continue;
                }
                else if (ch == OpenBrace || ReservedFormatCharacters.Contains(ch))
                {
                    parsingKey = false;
                    int idx;
                    if (!order.TryGetValue(key.ToString().Trim(), out idx))
                    {
                        throw new FormatException("Could not find key " + key);
                    }
                    key.Clear();
                    builder.Append(idx);
                    builder.Append(ch);
                    continue;
                }
                key.Append(ch);
            }

            return String.Format(builder.ToString(), values);
        }

        #endregion
    }
}
