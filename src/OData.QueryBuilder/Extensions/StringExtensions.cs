namespace OData.QueryBuilder.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrQuotes(this string value) =>
            string.IsNullOrEmpty(value) || value == "''";
    }
}
