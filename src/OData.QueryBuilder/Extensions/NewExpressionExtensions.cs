using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class NewExpressionExtensions
    {
        public static string ToODataQuery(this NewExpression newExpression)
        {
            if (newExpression.Members == default)
            {
                var arguments = new object[newExpression.Arguments.Count];

                for (var i = 0; i < newExpression.Arguments.Count; i++)
                {
                    arguments[i] = newExpression.Arguments[i].GetValue();
                }

                if (newExpression.Type == typeof(DateTime))
                {
                    var datetime = newExpression.Constructor.Invoke(arguments);

                    return $"{datetime:s}Z";
                }

                if (newExpression.Type == typeof(DateTimeOffset))
                {
                    var datetime = newExpression.Constructor.Invoke(arguments);

                    return $"{datetime:s}Z";
                }

                return string.Empty;
            }

            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }
    }
}
