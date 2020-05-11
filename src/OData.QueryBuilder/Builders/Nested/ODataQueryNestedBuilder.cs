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
        private ODataQueryNested _odataQueryNested;

        public ODataQueryNestedBuilder() =>
            _stringBuilder = new StringBuilder();

        public string Query => $"{_stringBuilder}({_odataQueryNested.Query})";

        public IODataQueryNestedParameter<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            if (!string.IsNullOrEmpty(_odataQueryNested?.Query))
            {
                _stringBuilder.Append($"({_odataQueryNested.Query}),{nestedEntityExpand.Body.ToODataQuery()}");
            }
            else
            {
                _stringBuilder.Append($"{nestedEntityExpand.Body.ToODataQuery()}");
            }

            _odataQueryNested = new ODataQueryNestedParameter<TNestedEntity>();

            return _odataQueryNested as ODataQueryNestedParameter<TNestedEntity>;
        }
    }
}
