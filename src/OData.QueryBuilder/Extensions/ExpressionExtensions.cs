using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionExtensions
    {
        public static string ToODataQuery(this Expression expression, string queryString)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    return binaryExpression.ToODataQuery(queryString);

                case MemberExpression memberExpression:
                    return memberExpression.ToODataQuery(queryString);

                case ConstantExpression constantExpression:
                    return constantExpression.ToODataQuery();

                case MethodCallExpression methodCallExpression:
                    return methodCallExpression.ToODataQuery(queryString);

                case NewExpression newExpression:
                    return newExpression.ToODataQuery();

                case UnaryExpression unaryExpression:
                    return unaryExpression.ToODataQuery(queryString);

                case LambdaExpression lambdaExpression:
                    return lambdaExpression.ToODataQuery();

                default:
                    return string.Empty;
            }
        }

        public static object GetValue(this Expression expression)
        {
            switch (expression)
            {
                case MemberExpression memberExpression:
                    return memberExpression.GetValue();

                case ConstantExpression constantExpression:
                    return constantExpression.GetValue();

                default:
                    return default;
            }
        }
    }
}
