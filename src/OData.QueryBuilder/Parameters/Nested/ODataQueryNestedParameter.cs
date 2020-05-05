using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.ExpressionVisitors;
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
            var visitor = new ExpandODataExpressionVisitor();

            visitor.Visit(entityNestedExpand.Body);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{builder.Query}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var visitor = new FilterODataExpressionVisitor();

            visitor.Visit(entityNestedFilter.Body);

            _queryBuilder.Append($"{Constants.QueryParameterFilter}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var visitor = new OrderByODataExpressionVisitor();

            visitor.Visit(entityNestedOrderBy.Body);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{visitor.Query} {Constants.QuerySortAsc}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var visitor = new OrderByODataExpressionVisitor();

            visitor.Visit(entityNestedOrderByDescending.Body);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{visitor.Query} {Constants.QuerySortDesc}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var visitor = new SelectODataExpressionVisitor();

            visitor.Visit(entityNestedSelect.Body);

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterTop}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringNestedSeparator}");

            return this;
        }
    }
}
