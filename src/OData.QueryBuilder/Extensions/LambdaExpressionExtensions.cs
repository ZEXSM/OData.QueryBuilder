using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class LambdaExpressionExtensions
    {
        public static string ToODataQuery(this LambdaExpression lambdaExpression)
        {
            var filter = lambdaExpression.Body.ToODataQuery();
            var tag = lambdaExpression.Parameters[0]?.Name;

            return $"{tag}:{tag}/{filter}";
        }
    }
}
