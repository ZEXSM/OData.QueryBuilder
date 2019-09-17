using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : IODataQueryParameterKey<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        private readonly Dictionary<string, string> _dicionaryBuilder;

        //public ODataQueryParameterKey(StringBuilder queryBuilder) => _queryBuilder = queryBuilder;

        public ODataQueryParameterKey(StringBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
            _dicionaryBuilder = new Dictionary<string, string>();
        }

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var entityExpandQuery = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={entityExpandQuery}&");
            _dicionaryBuilder.Add("$expand", entityExpandQuery);

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"$expand={odataQueryNestedBuilder.Query}&");
            _dicionaryBuilder.Add("$expand", odataQueryNestedBuilder.Query);

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var entitySelectQuery = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={entitySelectQuery}&");
            _dicionaryBuilder.Add("$select", entitySelectQuery);

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));

        public Dictionary<string, string> ToDictionary() => _dicionaryBuilder;
    }
}
