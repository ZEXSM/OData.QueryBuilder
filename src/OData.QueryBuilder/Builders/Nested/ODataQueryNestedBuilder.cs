using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Parameters.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders.Nested
{
    public class ODataQueryNestedBuilder<TEntity> : IODataQueryNestedBuilder<TEntity>
    {
        private ODataQueryNestedParameterBase _odataQueryNestedParameter;
        private string _resourceEntityName;

        public ODataQueryNestedBuilder()
        {
        }

        public string Query => $"{_resourceEntityName}({_odataQueryNestedParameter.Query})";

        public IODataQueryNestedParameter<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            _resourceEntityName = nestedEntityExpand.Body.ToODataQuery(string.Empty);

            _odataQueryNestedParameter = new ODataQueryNestedParameter<TNestedEntity>();

            return _odataQueryNestedParameter as ODataQueryNestedParameter<TNestedEntity>;
        }
    }
}
