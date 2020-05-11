using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class MethodCallExpressionExtensions
    {
        public static string ToODataQuery(this MethodCallExpression methodCallExpression, string queryString)
        {
            var methodName = methodCallExpression.Method.Name;
            var resource = default(object);
            var filter = default(string);

            switch (methodName)
            {
                case nameof(IODataQueryFunction.Date):
                    filter = methodCallExpression.Arguments[0]?.ToODataQuery(queryString) ?? string.Empty;

                    return $"{methodName.ToLower()}({filter})";

                case nameof(Enumerable.All):
                case nameof(Enumerable.Any):
                    resource = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                    filter = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                    return $"{resource}/{methodName.ToLower()}({filter})";

                case nameof(Enumerable.Contains):
                    if (methodCallExpression.Object == default(Expression))
                    {
                        resource = (methodCallExpression.Arguments[0] as MemberExpression).GetValue();
                        filter = methodCallExpression.Arguments[1].ToODataQuery(queryString);
                    }
                    else if (methodCallExpression.Object is MemberExpression)
                    {
                        resource = (methodCallExpression.Object as MemberExpression).GetValue() ??
                            methodCallExpression.Object.ToODataQuery(string.Empty);

                        filter = methodCallExpression.Arguments[0].ToODataQuery(queryString);
                    }
                    else if (methodCallExpression.Object is MethodCallExpression)
                    {
                        resource = methodCallExpression.Object.ToODataQuery(string.Empty);
                        filter = methodCallExpression.Arguments[0].ToODataQuery(string.Empty);
                    }

                    var inSequence = resource.GetInSequence();

                    if (inSequence != default)
                    {
                        if (!string.IsNullOrEmpty(inSequence))
                        {
                            return $"{filter} {inSequence}";
                        }
                        else
                        {
                            return $"{ODataQueryFunctions.Substringof}({filter},{resource})";
                        }
                    }

                    return string.Empty;

                case nameof(string.ToUpper):
                    return $"{ODataQueryFunctions.Toupper}({methodCallExpression.Object.ToODataQuery(string.Empty)})";

                case nameof(ToString):
                    return methodCallExpression.Object.ToODataQuery(string.Empty);

                default:
                    return string.Empty;
            }
        }

        private static string GetInSequence(this object arrayObj)
        {
            if (arrayObj == default)
            {
                return default;
            }
            if (arrayObj is IEnumerable<int>)
            {
                var inSequenceInt = string.Join(",", arrayObj as IEnumerable<int>);
                if (!string.IsNullOrEmpty(inSequenceInt))
                {
                    return $"{ODataQueryFunctions.In} ({inSequenceInt})";
                }
                else
                {
                    return default;
                }
            }

            if (arrayObj is IEnumerable<string>)
            {
                var inSequenceInt = string.Join("','", arrayObj as IEnumerable<string>);
                if (!string.IsNullOrEmpty(inSequenceInt))
                {
                    return $"{ODataQueryFunctions.In} ('{inSequenceInt}')";
                }
                else
                {
                    return default;
                }
            }

            return string.Empty;
        }
    }
}
