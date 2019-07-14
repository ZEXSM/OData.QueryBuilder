using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : IODataQueryParameterKey<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterKey(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var expandNames = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={expandNames}&");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"$expand={odataQueryNestedBuilder.Query}&");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var selectNames = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={selectNames}&");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
