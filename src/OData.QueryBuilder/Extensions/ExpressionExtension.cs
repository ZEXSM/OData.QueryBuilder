using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionExtension
    {
        public static object GetMemberExpressionValue(this MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression)
            {
                var constantValue = (memberExpression.Expression as ConstantExpression).Value;
                return memberExpression.Member.GetValue(constantValue);
            }

            if (memberExpression.Expression is MemberExpression)
            {
                var memberValue = GetMemberExpressionValue(memberExpression.Expression as MemberExpression);
                return memberExpression.Member.GetValue(memberValue);
            }

            return memberExpression.Member.GetValue(default(object));
        }

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

        public static string ToODataQuery(this LambdaExpression lambdaExpression)
        {
            var filter = lambdaExpression.Body.ToODataQuery(string.Empty);
            var tag = lambdaExpression.Parameters[0]?.Name;

            return $"{tag}:{tag}/{filter}";
        }

        public static string ToODataQuery(this ParameterExpression parameterExpression) =>
            parameterExpression.Name;

        public static string ToODataQuery(this NewExpression newExpression)
        {
            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }

        public static string FunctionDateToODataQuery(this BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.Type.Name == nameof(DateTime) || binaryExpression.Left.Type.Name == nameof(DateTimeOffset))
            {
                var leftExpression = binaryExpression.Left as MemberExpression;

                if (leftExpression != default(MemberExpression))
                {
                    if (leftExpression.ToODataQuery() == nameof(DateTime.Date)
                        && (leftExpression.Expression.Type.Name == nameof(DateTime) || leftExpression.Expression.Type.Name == nameof(DateTimeOffset)))
                    {
                        var leftQuery = leftExpression.Expression.ToODataQuery(string.Empty);

                        switch (binaryExpression.Right)
                        {
                            case NewExpression rightNewExpression:
                                var rightQueryNew = ((DateTime)rightNewExpression.Constructor
                                    .Invoke(rightNewExpression.Arguments.Select(s => ((ConstantExpression)s).Value).ToArray()))
                                    .ToString("yyyy-MM-dd");

                                return $"date({leftQuery}) {binaryExpression.NodeType.ToODataOperator()} {rightQueryNew}";
                            case MemberExpression rightMemberExpression:
                                var rightMemberQuery = ((DateTime)rightMemberExpression.GetMemberExpressionValue())
                                    .ToString("yyyy-MM-dd");

                                return $"date({leftQuery}) {binaryExpression.NodeType.ToODataOperator()} {rightMemberQuery}";
                        }
                    }
                    else
                    {
                        var leftQuery = leftExpression.ToODataQuery(string.Empty);

                        switch (binaryExpression.Right)
                        {
                            case UnaryExpression rightUnaryExpression:
                                var memberExpression = rightUnaryExpression.Operand as MemberExpression;
                                var rightQueryUnary = ((DateTime)memberExpression.GetMemberExpressionValue())
                                    .ToString("O");
                                return $"{leftQuery} {binaryExpression.NodeType.ToODataOperator()} {rightQueryUnary}";
                            case MemberExpression rightMemberExpression:
                                var rightQueryMember = ((DateTime)rightMemberExpression.GetMemberExpressionValue())
                                    .ToString("O");
                                return $"{leftQuery} {binaryExpression.NodeType.ToODataOperator()} {rightQueryMember}";
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static string ToODataQuery(this Expression expression, string queryString)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    var funcDateQuery = binaryExpression.FunctionDateToODataQuery();
                    if (!string.IsNullOrEmpty(funcDateQuery))
                    {
                        return funcDateQuery;
                    }

                    var leftQueryString = binaryExpression.Left.ToODataQuery(queryString);
                    var rightQueryString = binaryExpression.Right.ToODataQuery(queryString);

                    return $"{leftQueryString} {binaryExpression.NodeType.ToODataOperator()} {rightQueryString}";

                case MemberExpression memberExpression:
                    if (memberExpression.Expression is ConstantExpression)
                    {
                        return memberExpression.GetMemberExpressionValue().ToString();
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

                    if (methodName == nameof(Enumerable.Any) || methodName == nameof(Enumerable.All))
                    {
                        var resource = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                        var filter = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                        return $"{resource}/{methodName.ToLower()}({filter})";
                    }

                    if (methodName == nameof(Enumerable.Contains))
                    {
                        var resource = default(object);
                        var memberExpression = methodCallExpression.Arguments[0] as MemberExpression;

                        if (methodCallExpression.Object is MemberExpression)
                        {
                            resource = (methodCallExpression.Object as MemberExpression).GetMemberExpressionValue();

                            var filter = memberExpression.ToODataQuery(queryString);

                            if (resource is IEnumerable<int>)
                            {
                                return $"{filter} in ({string.Join(",", (IEnumerable<int>)resource)})";
                            }

                            if (resource is IEnumerable<string>)
                            {
                                return $"{filter} in ('{string.Join("','", (IEnumerable<string>)resource)}')";
                            }
                        }

                        if (memberExpression.Expression is MemberExpression)
                        {
                            resource = GetMemberExpressionValue(memberExpression);

                            var filter = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                            if (resource is IEnumerable<int>)
                            {
                                return $"{filter} in ({string.Join(",", (IEnumerable<int>)resource)})";
                            }

                            if (resource is IEnumerable<string>)
                            {
                                return $"{filter} in ('{string.Join("','", (IEnumerable<string>)resource)}')";
                            }
                        }
                    }

                    return $"{methodName.ToLower()}()";

                case NewExpression newExpression:
                    return newExpression.ToODataQuery();

                case UnaryExpression unaryExpression:
                    return unaryExpression.ToODataQuery();

                case LambdaExpression lambdaExpression:
                    return lambdaExpression.ToODataQuery();

                default:
                    return string.Empty;
            }
        }
    }
}
