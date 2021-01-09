using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class ExpressionTypeExtensions
    {
        public static string ToODataOperator(this ExpressionType expressionType) => expressionType switch
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
            _ => default,
        };
    }
}
