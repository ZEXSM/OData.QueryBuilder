using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : ODataQueryParameter, IODataQueryParameterKey<TEntity>
    {
        public ODataQueryParameterKey(StringBuilder queryBuilder)
            : base(queryBuilder)
        {
        }

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{builder.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{query}{Constants.QueryStringSeparator}");

            return this;
        }
    }
}
