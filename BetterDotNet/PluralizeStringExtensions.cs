namespace BetterDotNet
{
    /// <summary>
    /// String extensions for choosing a singular/plural form
    /// from an integer.
    /// </summary>
    public static class PluralizeStringExtensions
    {
        /// <summary>
        /// Choose either the singular or plural form of a word
        /// based on an integer value.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="singular">Singular version of the word.</param>
        /// <param name="plural">Plual version of the word.</param>
        /// <returns></returns>
        public static string Pluralize(this int num, string singular, string plural)
        {
            return num == 1 ? singular : plural;
        }

        /// <summary>
        /// Choose either the singular or plural form of a word
        /// based on a long value.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="singular">Singular version of the word.</param>
        /// <param name="plural">Plual version of the word.</param>
        /// <returns></returns>
        public static string Pluralize(this long num, string singular, string plural)
        {
            return num == 1L ? singular : plural;
        }
    }
}
