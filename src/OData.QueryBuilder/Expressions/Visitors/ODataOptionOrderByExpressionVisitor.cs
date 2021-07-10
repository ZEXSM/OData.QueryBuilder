using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Expressions.Visitors
{
    internal class ODataOptionOrderByExpressionVisitor : ODataOptionExpressionVisitor
    {
        public ODataOptionOrderByExpressionVisitor()
            : base()
        {
        }

        protected override string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(ISortFunction.Ascending):
                    var ascending0 = VisitExpression(methodCallExpression.Arguments[0]);

                    var ascendingQuery = VisitExpression(methodCallExpression.Object as MethodCallExpression);
                    var ascendingQueryComma = ascendingQuery == default ? string.Empty : QuerySeparators.Comma;

                    return $"{ascendingQuery}{ascendingQueryComma}{ascending0} {QuerySorts.Asc}";
                case nameof(ISortFunction.Descending):
                    var descending0 = VisitExpression(methodCallExpression.Arguments[0]);

                    var descendingQuery = VisitExpression(methodCallExpression.Object as MethodCallExpression);
                    var descendingQueryComma = descendingQuery == default ? string.Empty : QuerySeparators.Comma;

                    return $"{descendingQuery}{descendingQueryComma}{descending0} {QuerySorts.Desc}";
                default:
                    throw new NotSupportedException($"Method {methodCallExpression.Method.Name} not supported");
            }
        }
    }
}
