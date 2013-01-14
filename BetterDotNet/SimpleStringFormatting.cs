using System;

namespace BetterDotNet
{
    /// <summary>
    /// Various string formatting extensions.
    /// </summary>
    public static class SimpleStringFormatting
    {
        /// <summary>
        /// Format the input string with <paramref name="arg" /> as
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
    }
}
