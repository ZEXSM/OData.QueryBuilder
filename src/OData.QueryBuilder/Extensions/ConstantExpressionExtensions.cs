using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ConstantExpressionExtensions
    {
        public static string ToODataQuery(this ConstantExpression constantExpression) =>
            constantExpression.Value switch
            {
                bool b => b.ToString().ToLower(),
                int i => i.ToString(),
                string s => $"'{s}'",
                object o => $"'{o}'",
                _ => "null",
            };

        public static object GetValueOfConstantExpression(this ConstantExpression constantExpression) =>
            constantExpression.Value;
    }
}
