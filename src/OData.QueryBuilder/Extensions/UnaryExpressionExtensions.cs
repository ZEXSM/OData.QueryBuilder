using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class UnaryExpressionExtensions
    {
        public static string ToODataQuery(this UnaryExpression unaryExpression, string queryString)
        {
            var odataOperator = unaryExpression.NodeType.ToODataQueryOperator();
            odataOperator = !string.IsNullOrEmpty(odataOperator) ? $"{odataOperator} " : string.Empty;

            return $"{odataOperator}{unaryExpression.Operand.ToODataQuery(queryString)}";
        }
    }
}
