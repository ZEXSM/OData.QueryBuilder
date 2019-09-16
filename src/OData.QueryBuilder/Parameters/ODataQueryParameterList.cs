using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : IODataQueryParameterList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        private readonly Dictionary<string, string> _dicionaryBuilder;

        public ODataQueryParameterList(StringBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
            _dicionaryBuilder = new Dictionary<string, string>();
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var entityFilterQuery = entityFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$filter={entityFilterQuery}&");
            _dicionaryBuilder.Add("$filter", entityFilterQuery);

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var entityExpandQuery = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={entityExpandQuery}&");
            _dicionaryBuilder.Add("$expand", entityExpandQuery);

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"$expand={odataQueryNestedBuilder.Query}&");
            _dicionaryBuilder.Add("$expand", odataQueryNestedBuilder.Query);

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var entitySelectQuery = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={entitySelectQuery}&");
            _dicionaryBuilder.Add("$select", entitySelectQuery);

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var entityOrderByQuery = entityOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={entityOrderByQuery} asc&");
            _dicionaryBuilder.Add("$orderby", entityOrderByQuery + " asc");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var entityOrderByDescendingQuery = entityOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={entityOrderByDescendingQuery} desc&");
            _dicionaryBuilder.Add("$orderby", entityOrderByDescendingQuery + " desc");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int number)
        {
            _queryBuilder.Append($"$skip={number}&");
            _dicionaryBuilder.Add("$skip", number.ToString());

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int number)
        {
            _queryBuilder.Append($"$top={number}&");
            _dicionaryBuilder.Add("$top", number.ToString());

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"$count={value.ToString().ToLower()}&");
            _dicionaryBuilder.Add("$count", value.ToString().ToLower());

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));

        public Dictionary<string, string> ToDicionary() => _dicionaryBuilder;
    }
}
