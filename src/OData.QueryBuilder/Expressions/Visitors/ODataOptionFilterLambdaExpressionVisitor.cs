using System;
using OData.QueryBuilder.Options;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Expressions.Visitors
{
    internal class ODataOptionFilterLambdaExpressionVisitor : ODataOptionFilterExpressionVisitor
    {
        public ODataOptionFilterLambdaExpressionVisitor(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(odataQueryBuilderOptions)
        {
        }

        protected override string VisitParameterExpression(LambdaExpression topExpression, ParameterExpression parameterExpression) =>
            parameterExpression.Name;
    }
}
