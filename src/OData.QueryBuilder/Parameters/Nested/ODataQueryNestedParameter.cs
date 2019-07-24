using OData.QueryBuilder.Builders.Nested;
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

        public IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityExpandNested)
        {
            var entityExpandNestedQuery = entityExpandNested.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={entityExpandNestedQuery};");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"$expand={odataQueryNestedBuilder.Query};");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var entityNestedFilterQuery = entityNestedFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$filter={entityNestedFilterQuery};");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entitySelectNested)
        {
            var entitySelectNestedQuery = entitySelectNested.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={entitySelectNestedQuery};");

            return this;
        }
    }
}
