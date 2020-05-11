using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ConstantExpressionExtensions
    {
        public static string ToODataQuery(this ConstantExpression constantExpression)
        {
            switch (constantExpression.Value)
            {
                case bool b:
                    return b.ToString().ToLower();
                case int i:
                    return i.ToString();
                case string s:
                    return $"'{s}'";
                case object o:
                    return $"'{o}'";
                default:
                    return "null";
            }
        }

        public static object GetValue(this ConstantExpression constantExpression) =>
            constantExpression.Value;
    }
}
