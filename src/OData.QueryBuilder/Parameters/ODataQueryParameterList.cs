using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : IODataQueryParameterList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterList(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var entityFilterQuery = entityFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$filter={entityFilterQuery}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var entityExpandQuery = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={entityExpandQuery}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"$expand={odataQueryNestedBuilder.Query}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var entitySelectQuery = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={entitySelectQuery}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var entityOrderByQuery = entityOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={entityOrderByQuery} asc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var entityOrderByDescendingQuery = entityOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={entityOrderByDescendingQuery} desc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int number)
        {
            _queryBuilder.Append($"$skip={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int number)
        {
            _queryBuilder.Append($"$top={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"$count={value.ToString().ToLower()}&");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
