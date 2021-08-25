using System.Globalization;
using System.Text;

namespace Yerbowo.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove Diacritics from a string
        /// </summary>
        /// <returns>
        /// The string without diacritics.
        /// </returns>
        /// <param name="s">The string with polish signs</param>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            char sign;

            foreach (var c in normalizedString)
            {
                sign = NormaliseLWithStroke(c);

                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(sign);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(sign);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// This is an additional helper that normalizes polish character. 
        /// </summary>
        private static char NormaliseLWithStroke(char c)
        {
            switch (c)
            {
                case 'ł':
                    return 'l';
                case 'Ł':
                    return 'L';
                default:
                    return c;
            }
        }

        /// <summary>
        /// Removes Diacritics, replaces empty spaces for dashes and changes string to lowercaase.
        /// </summary>
        /// <returns>
        /// The Slug.
        /// </returns>
        /// <param name="s">The string with diacritics, for example polish signs</param>
        public static string ToSlug(this string text)
        {
            return text.RemoveDiacritics().Replace(" ", "-").ToLower();
        }

        public static string ToTitle(this string text)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
        }
    }
}
