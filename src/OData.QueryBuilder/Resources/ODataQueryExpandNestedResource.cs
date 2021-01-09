using OData.QueryBuilder.Conventions.Options.Nested;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
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
        private readonly ODataResourceExpressionVisitor _odataQueryResourceExpressionVisitor;

        public string Query => $"{_stringBuilder}({_odataOptionNestedBase.Query})";

        public ODataQueryExpandNestedResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _odataQueryResourceExpressionVisitor = new ODataResourceExpressionVisitor();
        }

        public IODataOptionNested<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            var query = _odataQueryResourceExpressionVisitor.ToQuery(nestedEntityExpand.Body);

            if (!string.IsNullOrEmpty(_odataOptionNestedBase?.Query))
            {
                _stringBuilder.Append($"({_odataOptionNestedBase.Query}),{query}");
            }
            else
            {
                _stringBuilder.Append(query);
            }

            _odataOptionNestedBase = new ODataOptionNested<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataOptionNestedBase as ODataOptionNested<TNestedEntity>;
        }
    }
}
