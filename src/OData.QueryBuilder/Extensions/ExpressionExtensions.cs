using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionExtensions
    {
        public static string ToODataQuery(this Expression expression, string queryString = "") =>
            expression switch
            {
                BinaryExpression binaryExpression => binaryExpression.ToODataQuery(queryString),
                MemberExpression memberExpression => memberExpression.ToODataQuery(queryString),
                ConstantExpression constantExpression => constantExpression.ToODataQuery(),
                MethodCallExpression methodCallExpression => methodCallExpression.ToODataQuery(queryString),
                NewExpression newExpression => newExpression.ToODataQuery(),
                UnaryExpression unaryExpression => unaryExpression.ToODataQuery(queryString),
                LambdaExpression lambdaExpression => lambdaExpression.ToODataQuery(),
                _ => string.Empty,
            };

        public static object GetValueOfExpression(this Expression expression) =>
            expression switch
            {
                MemberExpression memberExpression => memberExpression.GetValueOfMemberExpression(),
                ConstantExpression constantExpression => constantExpression.GetValueOfConstantExpression(),
                _ => default,
            };
    }
}
