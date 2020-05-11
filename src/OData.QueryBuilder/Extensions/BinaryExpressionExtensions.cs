using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class BinaryExpressionExtensions
    {
        public static string ToODataQuery(this BinaryExpression binaryExpression, string queryString)
        {
            var leftQueryString = binaryExpression.Left.ToODataQuery(queryString);
            var rightQueryString = binaryExpression.Right.ToODataQuery(queryString);

            if (string.IsNullOrEmpty(leftQueryString))
            {
                return rightQueryString;
            }

            if (string.IsNullOrEmpty(rightQueryString))
            {
                return leftQueryString;
            }

            return $"{leftQueryString} {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryString}";
        }
    }
}
