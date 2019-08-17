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

        public static string GetInSequence(this object arrayObj)
        {
            if (arrayObj is IEnumerable<int>)
            {
                var inSequenceInt = string.Join(",", arrayObj as IEnumerable<int>);
                if (!string.IsNullOrEmpty(inSequenceInt))
                {
                    return $"in ({inSequenceInt})";
                }
            }

            if (arrayObj is IEnumerable<string>)
            {
                var inSequenceInt = string.Join("','", arrayObj as IEnumerable<string>);
                if (!string.IsNullOrEmpty(inSequenceInt))
                {
                    return $"in ('{inSequenceInt}')";
                }
            }

            return string.Empty;
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
                case ExpressionType.Not:
                    return "not";
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

        public static string ToODataQuery(this ConstantExpression constantExpression)
        {
            switch (constantExpression.Value)
            {
                case bool boolVal:
                    return boolVal.ToString().ToLower();
                case string stringVal:
                    return $"'{stringVal}'";
                case int intVal:
                    return intVal.ToString();
                default:
                    return "null";
            }
        }

        public static string ToODataQuery(this MemberExpression memberExpression) =>
            memberExpression.Member.Name;

        public static string ToODataQuery(this LambdaExpression lambdaExpression)
        {
            var filter = lambdaExpression.Body.ToODataQuery(string.Empty);
            var tag = lambdaExpression.Parameters[0]?.Name;

            return $"{tag}:{tag}/{filter}";
        }

        public static string ToODataQuery(this NewExpression newExpression)
        {
            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }

        public static string ToODataQueryFunctionDate(this BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left.Type == typeof(DateTime) || binaryExpression.Left.Type == typeof(DateTimeOffset)
                || binaryExpression.Left.Type == typeof(DateTime?) || binaryExpression.Left.Type == typeof(DateTimeOffset?))
            {
                var leftExpression = binaryExpression.Left as MemberExpression ??
                    (binaryExpression.Left as UnaryExpression).Operand as MemberExpression ??
                    ((binaryExpression.Left as UnaryExpression).Operand as UnaryExpression).Operand as MemberExpression;

                if (leftExpression != default(MemberExpression))
                {
                    if (leftExpression.ToODataQuery() == nameof(DateTime.Date))
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
                                var rightQueryMember = ((DateTime)rightMemberExpression.GetMemberExpressionValue())
                                    .ToString("yyyy-MM-dd");

                                return $"date({leftQuery}) {binaryExpression.NodeType.ToODataOperator()} {rightQueryMember}";
                            case ConstantExpression rightConstantExpression:
                                var rightQueryConstant = rightConstantExpression.ToODataQuery();

                                return $"date({leftQuery}) {binaryExpression.NodeType.ToODataOperator()} {rightQueryConstant}";
                        }
                    }
                    else
                    {
                        var leftQuery = leftExpression.ToODataQuery(string.Empty);

                        switch (binaryExpression.Right)
                        {
                            case UnaryExpression rightUnaryExpression:
                                var memberExpression = rightUnaryExpression.Operand as MemberExpression ??
                                    (rightUnaryExpression.Operand as UnaryExpression).Operand as MemberExpression;

                                var rightQueryUnary = ((DateTime)memberExpression.GetMemberExpressionValue())
                                    .ToString("O");

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataOperator()} {rightQueryUnary}";
                            case MemberExpression rightMemberExpression:
                                var rightQueryMember = ((DateTime)rightMemberExpression.GetMemberExpressionValue())
                                    .ToString("O");

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataOperator()} {rightQueryMember}";
                            case ConstantExpression rightConstantExpression:
                                var rightQueryConstant = rightConstantExpression.ToODataQuery();

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataOperator()} {rightQueryConstant}";
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
                    var funcDateQuery = binaryExpression.ToODataQueryFunctionDate();
                    if (!string.IsNullOrEmpty(funcDateQuery))
                    {
                        return funcDateQuery;
                    }

                    var leftQueryString = binaryExpression.Left.ToODataQuery(queryString);
                    var rightQueryString = binaryExpression.Right.ToODataQuery(queryString);

                    if (string.IsNullOrEmpty(leftQueryString))
                    {
                        return rightQueryString;
                    }

                    if (string.IsNullOrEmpty(rightQueryString))
                    {
                        return leftQueryString;
                    }

                    return $"{leftQueryString} {binaryExpression.NodeType.ToODataOperator()} {rightQueryString}";

                case MemberExpression memberExpression:
                    var memberExpressionValue = memberExpression.GetMemberExpressionValue();

                    if (memberExpressionValue != default(object))
                    {
                        if (memberExpressionValue is string)
                        {
                            return $"'{memberExpressionValue}'";
                        }

                        return memberExpressionValue.ToString();
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
                        var filter = default(string);

                        if (methodCallExpression.Object == default(Expression))
                        {
                            resource = (methodCallExpression.Arguments[0] as MemberExpression).GetMemberExpressionValue();
                            filter = methodCallExpression.Arguments[1].ToODataQuery(queryString);
                        }
                        else if (methodCallExpression.Object is MemberExpression)
                        {
                            resource = (methodCallExpression.Object as MemberExpression).GetMemberExpressionValue();
                            filter = methodCallExpression.Arguments[0].ToODataQuery(queryString);
                        }

                        var inSequence = resource.GetInSequence();

                        if (!string.IsNullOrEmpty(inSequence))
                        {
                            return $"{filter} {inSequence}";
                        }
                    }

                    return string.Empty;

                case NewExpression newExpression:
                    return newExpression.ToODataQuery();

                case UnaryExpression unaryExpression:
                    var odataOperator = unaryExpression.NodeType.ToODataOperator();
                    odataOperator = !string.IsNullOrEmpty(odataOperator) ? $"{odataOperator} " : string.Empty;

                    return $"{odataOperator}{unaryExpression.Operand.ToODataQuery(queryString)}";

                case LambdaExpression lambdaExpression:
                    return lambdaExpression.ToODataQuery();

                default:
                    return string.Empty;
            }
        }
    }
}
