using System.Text.RegularExpressions;

namespace SampleApi.Extensions
{
    public static partial class StringExtensions
    {
        // Use a generated regex for leading underscores
        [GeneratedRegex(@"^_+")]
        private static partial Regex StartUnderscoresRegex();

        // Use a generated regex for camelCase boundary pattern
        [GeneratedRegex(@"([a-z0-9])([A-Z])")]
        private static partial Regex CamelCaseBoundaryRegex();

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Match leading underscores (if any) and keep them
            var startUnderscores = StartUnderscoresRegex().Match(input).Value;

            // Replace camel boundaries with underscore between groups
            var replaced = CamelCaseBoundaryRegex().Replace(input, "$1_$2");

            // Lowercase the result and prefix the start underscores (if any)
            return startUnderscores + replaced.ToLower();
        }
    }
}
