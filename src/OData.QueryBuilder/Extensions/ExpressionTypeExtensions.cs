using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionTypeExtensions
    {
        public static string ToODataQueryOperator(this ExpressionType expressionType) =>
            expressionType switch
            {
                ExpressionType.And => "and",
                ExpressionType.AndAlso => "and",
                ExpressionType.Or => "or",
                ExpressionType.OrElse => "or",
                ExpressionType.Equal => "eq",
                ExpressionType.Not => "not",
                ExpressionType.NotEqual => "ne",
                ExpressionType.LessThan => "lt",
                ExpressionType.LessThanOrEqual => "le",
                ExpressionType.GreaterThan => "gt",
                ExpressionType.GreaterThanOrEqual => "ge",
                _ => string.Empty,
            };
    }
}
