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

            _queryBuilder.Append($"{Contants.QueryParameterExpand}{Contants.QueryStringEqualSign}{odataNestedExpandQuery}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"{Contants.QueryParameterExpand}{Contants.QueryStringEqualSign}{odataQueryNestedBuilder.Query}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedFilter.Body);

            var odataNestedFilterQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterFilter}{Contants.QueryStringEqualSign}{odataNestedFilterQuery}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedOrderBy.Body);

            var odataNestedOrderByQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterOrderBy}{Contants.QueryStringEqualSign}{odataNestedOrderByQuery} {Contants.QuerySortAsc}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedOrderByDescending.Body);

            var odataNestedOrderByDescendingQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterOrderBy}{Contants.QueryStringEqualSign}{odataNestedOrderByDescendingQuery} {Contants.QuerySortDesc}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityNestedSelect.Body);

            var odataNestedSelectQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterSelect}{Contants.QueryStringEqualSign}{odataNestedSelectQuery}{Contants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Contants.QueryParameterTop}{Contants.QueryStringEqualSign}{value}{Contants.QueryStringNestedSeparator}");

            return this;
        }
    }
}
