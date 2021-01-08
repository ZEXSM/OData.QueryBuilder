using OData.QueryBuilder.Conventions.Options.Nested;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Resources
{
    internal class ODataQueryExpandNestedResource<TEntity> : IODataQueryExpandNestedResource<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        private ODataOptionNestedBase _odataOptionNestedBase;
        private readonly QueryExpressionVisitor _visitorExpression;

        public string Query => $"{_stringBuilder}({_odataOptionNestedBase.Query})";

        public ODataQueryExpandNestedResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _visitorExpression = new QueryExpressionVisitor(_odataQueryBuilderOptions);
        }

        public IODataOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            if (!string.IsNullOrEmpty(_odataOptionNestedBase?.Query))
            {
                var query = _visitorExpression.ToString(nestedEntityExpand.Body);

                _stringBuilder.Append($"({_odataOptionNestedBase.Query}),{query}");
            }
            else
            {
                var query = _visitorExpression.ToString(nestedEntityExpand.Body);

                _stringBuilder.Append(query);
            }

            _odataOptionNestedBase = new ODataOptionNested<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataOptionNestedBase as ODataOptionNested<TNestedEntity>;
        }
    }
}
