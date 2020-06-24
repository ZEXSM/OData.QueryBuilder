using OData.QueryBuilder.Conventions.Options.Nested;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryNestedBuilder<TEntity> : IODataQueryNestedBuilder<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        private ODataQueryNested _odataQueryNested;
        internal readonly VisitorExpression _visitorExpression;

        public ODataQueryNestedBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _visitorExpression = new VisitorExpression();
        }

        public string Query => $"{_stringBuilder}({_odataQueryNested.Query})";

        public IODataOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            if (!string.IsNullOrEmpty(_odataQueryNested?.Query))
            {
                var query = _visitorExpression.ToString(nestedEntityExpand.Body);

                _stringBuilder.Append($"({_odataQueryNested.Query}),{query}");
            }
            else
            {
                var query = _visitorExpression.ToString(nestedEntityExpand.Body);

                _stringBuilder.Append(query);
            }

            _odataQueryNested = new ODataOptionNested<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataQueryNested as ODataOptionNested<TNestedEntity>;
        }
    }
}
