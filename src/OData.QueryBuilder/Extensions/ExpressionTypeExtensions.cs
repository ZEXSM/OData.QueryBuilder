using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionTypeExtensions
    {
        public static string ToODataQueryOperator(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "eq";
                case ExpressionType.Not:
                    return "not";
                case ExpressionType.NotEqual:
                    return "ne";
                case ExpressionType.LessThan:
                    return "lt";
                case ExpressionType.LessThanOrEqual:
                    return "le";
                case ExpressionType.GreaterThan:
                    return "gt";
                case ExpressionType.GreaterThanOrEqual:
                    return "ge";
                default:
                    return string.Empty;
            }
        }
    }
}
