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

        protected override string VisitMethodCallExpression(LambdaExpression topExpression, MethodCallExpression methodCallExpression)
        {
            var method = methodCallExpression.Method;
            if (method.DeclaringType!.IsAssignableFrom(typeof(ISortFunction)))
            {
                switch (method.Name)
                {
                    case nameof(ISortFunction.Ascending):
                        var ascending0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        var ascendingQuery = VisitExpression(topExpression,
                            methodCallExpression.Object as MethodCallExpression);
                        var ascendingQueryComma =
                            ascendingQuery == default ? string.Empty : QuerySeparators.StringComma;

                        return $"{ascendingQuery}{ascendingQueryComma}{ascending0} {QuerySorts.Asc}";
                    case nameof(ISortFunction.Descending):
                        var descending0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        var descendingQuery = VisitExpression(topExpression,
                            methodCallExpression.Object as MethodCallExpression);
                        var descendingQueryComma =
                            descendingQuery == default ? string.Empty : QuerySeparators.StringComma;

                        return $"{descendingQuery}{descendingQueryComma}{descending0} {QuerySorts.Desc}";
                }
            }

            return base.VisitMethodCallExpression(topExpression, methodCallExpression);
        }
    }
}
