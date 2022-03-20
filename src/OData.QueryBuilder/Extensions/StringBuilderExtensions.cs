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

        public static StringBuilder Merge(this StringBuilder stringBuilder, string startValue, char endChar, string value)
        {
            var positionEndFilter = -1;

            for (var position = stringBuilder.Length - 1; position >= 0; position--)
            {
                if (stringBuilder[position] == endChar)
                {
                    positionEndFilter = position;

                    continue;
                }

                if (stringBuilder[position] == startValue[0]
                    && stringBuilder[position + 1] == startValue[1])
                {
                    stringBuilder.Insert(positionEndFilter, value);

                    break;
                }
            }

            return stringBuilder;
        }
    }
}
