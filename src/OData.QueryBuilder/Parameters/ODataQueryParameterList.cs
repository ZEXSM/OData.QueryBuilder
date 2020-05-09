using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
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

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterFilter}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterExpand}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(builder);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterExpand}{ODataQuerySeparators.QueryStringEqualSign}{builder.Query}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterSelect}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var query = entityOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterOrderBy}{ODataQuerySeparators.QueryStringEqualSign}{query} {ODataQuerySorts.QuerySortAsc}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var query = entityOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterOrderBy}{ODataQuerySeparators.QueryStringEqualSign}{query} {ODataQuerySorts.QuerySortDesc}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int value)
        {
            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterSkip}{ODataQuerySeparators.QueryStringEqualSign}{value}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterTop}{ODataQuerySeparators.QueryStringEqualSign}{value}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterCount}{ODataQuerySeparators.QueryStringEqualSign}{value.ToString().ToLower()}{ODataQuerySeparators.QueryStringSeparator}");

            return this;
        }
    }
}
