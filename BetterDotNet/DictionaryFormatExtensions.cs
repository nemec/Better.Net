using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterDotNet
{
    /// <summary>
    /// Extensions for formatting based on dictionary keys
    /// </summary>
    public static class DictionaryFormatExtensions
    {
        private const string ReservedFormatCharacters = "{},:";
        private const char OpenBrace = '{';

        /// <summary>
        /// Format the input string. Instead of using positional
        /// arguments to assign values, the keys of the dictionary are
        /// used to determine which value is placed where.
        /// </summary>
        /// <param name="fmt">Format string</param>
        /// <param name="dict">Dictionary containing keys to substitute</param>
        /// <returns></returns>
        public static string Format(this string fmt, IDictionary<string, object> dict)
        {
            return Format(fmt, null, dict);
        }

        /// <summary>
        /// Format the input string using the given
        /// <see cref="IFormatProvider"/>. Instead of using positional
        /// arguments to assign values, the keys of the dictionary are
        /// used to determine which value is placed where.
        /// </summary>
        /// <param name="fmt">Format string</param>
        /// <param name="provider">A custom format provider</param>
        /// <param name="dict">Dictionary containing keys to substitute</param>
        /// <returns></returns>
        public static string Format(this string fmt, IFormatProvider provider, IDictionary<string, object> dict)
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

            return provider == null ?
                String.Format(builder.ToString(), values) :
                String.Format(builder.ToString(), provider, values);
        }
    }
}
