using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class NewExpressionExtensions
    {
        public static string ToODataQuery(this NewExpression newExpression)
        {
            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }
    }
}
