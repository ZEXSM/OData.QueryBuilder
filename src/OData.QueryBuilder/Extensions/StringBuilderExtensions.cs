using System.Text;

namespace OData.QueryBuilder.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder LastRemove(this StringBuilder stringBuilder, char @char)
        {
            if (stringBuilder.Length == 0)
            {
                return stringBuilder;
            }

            var lastIndex = stringBuilder.Length - 1;

            if (stringBuilder[lastIndex] == @char)
            {
                stringBuilder.Remove(lastIndex, 1);
            }

            return stringBuilder;
        }

        public static StringBuilder LastReplace(this StringBuilder stringBuilder, char oldChar, char newChar)
        {
            var lastIndex = stringBuilder.Length - 1;

            if (stringBuilder[lastIndex] == oldChar)
            {
                stringBuilder[lastIndex] = newChar;
            }

            return stringBuilder;
        }
    }
}
