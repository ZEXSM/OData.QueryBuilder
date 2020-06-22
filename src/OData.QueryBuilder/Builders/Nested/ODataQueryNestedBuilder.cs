using OData.QueryBuilder.V4.Options.Nested;
using OData.QueryBuilder.Visitors;
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

        public IODataQueryOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            if (!string.IsNullOrEmpty(_odataQueryNested?.Query))
            {
                var visitor = new VisitorExpression(nestedEntityExpand.Body);
                var query = visitor.ToString();

                _stringBuilder.Append($"({_odataQueryNested.Query}),{query}");
            }
            else
            {
                var visitor = new VisitorExpression(nestedEntityExpand.Body);
                var query = visitor.ToString();

                _stringBuilder.Append(query);
            }

            _odataQueryNested = new ODataQueryOptionNested<TNestedEntity>();

            return _odataQueryNested as ODataQueryOptionNested<TNestedEntity>;
        }
    }
}
