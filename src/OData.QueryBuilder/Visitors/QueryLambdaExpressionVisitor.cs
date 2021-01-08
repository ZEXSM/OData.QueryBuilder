using OData.QueryBuilder.Options;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Visitors
{
    internal class QueryLambdaExpressionVisitor : QueryExpressionVisitor
    {
        public QueryLambdaExpressionVisitor(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(odataQueryBuilderOptions)
        {
        }

        protected override string VisitParameterExpression(ParameterExpression parameterExpression) =>
            parameterExpression.Name;
    }
}
