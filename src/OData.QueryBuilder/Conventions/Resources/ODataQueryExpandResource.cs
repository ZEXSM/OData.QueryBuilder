using OData.QueryBuilder.Conventions.Options.Expand;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Resources
{
    internal class ODataQueryExpandResource<TEntity> : IODataQueryExpandResource<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        private ODataOptionExpandBase _odataOptionNestedBase;

        public string Query => $"{_stringBuilder}({_odataOptionNestedBase.Query})";

        public ODataQueryExpandResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataOptionExpand<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedEntityExpand)
        {
            var query = new ODataResourceExpressionVisitor().ToQuery(nestedEntityExpand.Body);

            if (!string.IsNullOrEmpty(_odataOptionNestedBase?.Query))
            {
                _stringBuilder.Append($"({_odataOptionNestedBase.Query}),{query}");
            }
            else
            {
                _stringBuilder.Append(query);
            }

            _odataOptionNestedBase = new ODataOptionExpand<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataOptionNestedBase as ODataOptionExpand<TNestedEntity>;
        }
    }
}
