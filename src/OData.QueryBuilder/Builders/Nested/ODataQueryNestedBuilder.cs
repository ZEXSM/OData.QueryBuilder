using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Parameters.Nested;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Builders.Nested
{
    public class ODataQueryNestedBuilder<TEntity> : IODataQueryNestedBuilder<TEntity>
    {
        private readonly StringBuilder _stringBuilder;
        private ODataQueryNestedParameterBase _odataQueryNestedParameter;

        public ODataQueryNestedBuilder() =>
            _stringBuilder = new StringBuilder();

        public string Query => $"{_stringBuilder}({_odataQueryNestedParameter.Query})";

        public IODataQueryNestedParameter<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            if (!string.IsNullOrEmpty(_odataQueryNestedParameter?.Query))
            {
                _stringBuilder.Append($"({_odataQueryNestedParameter.Query}),{nestedEntityExpand.Body.ToODataQuery(string.Empty)}");
            }
            else
            {
                _stringBuilder.Append($"{nestedEntityExpand.Body.ToODataQuery(string.Empty)}");
            }

            _odataQueryNestedParameter = new ODataQueryNestedParameter<TNestedEntity>();

            return _odataQueryNestedParameter as ODataQueryNestedParameter<TNestedEntity>;
        }
    }
}
