using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionExtension
    {
        public static string ToODataOperator(this ExpressionType expressionType)
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

        public static string ToODataQuery(this ConstantExpression constantExpression) =>
            constantExpression.Value?.ToString() ?? "null";

        public static string ToODataQuery(this UnaryExpression unaryExpression) =>
            ((MemberExpression)unaryExpression.Operand).Member.Name;

        public static string ToODataQuery(this MemberExpression memberExpression) =>
            memberExpression.Member.Name;

        public static string ToODataQuery(this NewExpression newExpression)
        {
            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }

        public static string ToODataQuery(this Expression expression, string queryString)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    var leftQueryString = binaryExpression.Left.ToODataQuery(queryString);
                    var rightQueryString = binaryExpression.Right.ToODataQuery(queryString);

                    return $"{leftQueryString} {binaryExpression.NodeType.ToODataOperator()} {rightQueryString}";

                case MemberExpression memberExpression:
                    if (memberExpression.Expression is ConstantExpression)
                    {
                        if (memberExpression.Member is FieldInfo)
                        {
                            var valueConstantExpression = ((FieldInfo)memberExpression.Member).GetValue(((ConstantExpression)memberExpression.Expression).Value);

                            if (valueConstantExpression is IEnumerable<int>)
                            {
                                return string.Join(",", (IEnumerable<int>)valueConstantExpression);
                            }

                            if (valueConstantExpression is IEnumerable<string>)
                            {
                                return $"'{string.Join("','", (IEnumerable<string>)valueConstantExpression)}'";
                            }

                            return valueConstantExpression.ToString();
                        }
                    }

                    var parentMemberExpressionQuery = memberExpression.Expression.ToODataQuery(queryString);

                    if (string.IsNullOrEmpty(parentMemberExpressionQuery))
                    {
                        return memberExpression.ToODataQuery();
                    }

                    return $"{parentMemberExpressionQuery}/{memberExpression.ToODataQuery()}";

                case ConstantExpression constantExpression:
                    return constantExpression.ToODataQuery();

                case MethodCallExpression methodCallExpression:
                    var methodName = methodCallExpression.Method.Name;
                    var methodParameters = methodCallExpression.Arguments[0].ToODataQuery(queryString);

                    if (methodName == nameof(string.Contains))
                    {

                        var valueConstantExpression = methodCallExpression.Object.ToODataQuery(queryString);


                        return $"{methodParameters} in ({valueConstantExpression})";
                    }

                    return $"{methodName.ToLower()}({methodParameters})";

                default:
                    return string.Empty;
            }
        }
    }
}
