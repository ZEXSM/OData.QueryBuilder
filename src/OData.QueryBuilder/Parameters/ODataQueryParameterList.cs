using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : ODataQueryParameter, IODataQueryParameterList<TEntity>
    {
        public ODataQueryParameterList(StringBuilder queryBuilder) :
            base(queryBuilder)
        {
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var query = entityFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterFilter}{Constants.QueryStringEqualSign}{query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(builder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{builder.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var query = entityOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{query} {Constants.QuerySortAsc}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var query = entityOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{query} {Constants.QuerySortDesc}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterSkip}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterTop}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"{Constants.QueryParameterCount}{Constants.QueryStringEqualSign}{value.ToString().ToLower()}{Constants.QueryStringSeparator}");

            return this;
        }
    }
}
