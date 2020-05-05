using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Expressions;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters.Nested
{
    public class ODataQueryNestedParameter<TEntity> : ODataQueryNestedParameterBase, IODataQueryNestedParameter<TEntity>
    {
        public ODataQueryNestedParameter()
            : base()
        {
        }

        public IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedExpand.Body);

            var odataNestedExpandQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataNestedExpandQuery}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataQueryNestedBuilder.Query}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedFilter.Body);

            var odataNestedFilterQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterFilter}{Constants.QueryStringEqualSign}{odataNestedFilterQuery}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedOrderBy.Body);

            var odataNestedOrderByQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{odataNestedOrderByQuery} {Constants.QuerySortAsc}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedOrderByDescending.Body);

            var odataNestedOrderByDescendingQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{odataNestedOrderByDescendingQuery} {Constants.QuerySortDesc}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedSelect.Body);

            var odataNestedSelectQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{odataNestedSelectQuery}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterTop}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringNestedSeparator}");

            return this;
        }
    }
}
