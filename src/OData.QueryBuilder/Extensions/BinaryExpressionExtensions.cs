using OData.QueryBuilder.Constants;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class BinaryExpressionExtensions
    {
        public static string ToODataQuery(this BinaryExpression binaryExpression, string queryString)
        {
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

            return $"{leftQueryString} {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryString}";
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
                    if (leftExpression.Member.Name == nameof(DateTime.Date))
                    {
                        var leftQuery = leftExpression.Expression.ToODataQuery(string.Empty);

                        switch (binaryExpression.Right)
                        {
                            case NewExpression rightNewExpression:
                                var rightQueryNew = ((DateTime)rightNewExpression.Constructor
                                    .Invoke(rightNewExpression.Arguments.Select(s => ((ConstantExpression)s).Value).ToArray()))
                                    .ToString("yyyy-MM-dd");

                                return $"{ODataQueryFunctions.Date}({leftQuery}) {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryNew}";
                            case MemberExpression rightMemberExpression:
                                var rightQueryMember = ((DateTime)rightMemberExpression.GetMemberValue())
                                    .ToString("yyyy-MM-dd");

                                return $"{ODataQueryFunctions.Date}({leftQuery}) {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryMember}";
                            case ConstantExpression rightConstantExpression:
                                var rightQueryConstant = rightConstantExpression.ToODataQuery();

                                return $"{ODataQueryFunctions.Date}({leftQuery}) {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryConstant}";
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

                                var rightQueryUnary = ((DateTime)memberExpression.GetMemberValue())
                                    .ToString("O");

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryUnary}";
                            case MemberExpression rightMemberExpression:
                                var rightQueryMember = ((DateTime)rightMemberExpression.GetMemberValue())
                                    .ToString("O");

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryMember}";
                            case ConstantExpression rightConstantExpression:
                                var rightQueryConstant = rightConstantExpression.ToODataQuery();

                                return $"{leftQuery} {binaryExpression.NodeType.ToODataQueryOperator()} {rightQueryConstant}";
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
