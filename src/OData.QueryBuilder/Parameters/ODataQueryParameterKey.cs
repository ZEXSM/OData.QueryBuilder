using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
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

            _queryBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _queryBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{builder.Query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.Select}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }
    }
}
