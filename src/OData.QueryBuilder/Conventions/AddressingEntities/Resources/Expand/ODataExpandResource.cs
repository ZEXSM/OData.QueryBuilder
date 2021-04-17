using OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand
{
    internal class ODataExpandResource<TEntity> : IODataExpandResource<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        private ODataQueryExpandBase _odataOptionNestedBase;

        public string Query => $"{_stringBuilder}({_odataOptionNestedBase.Query})";

        public ODataExpandResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataQueryExpand<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedExpand)
        {
            var query = new ODataResourceExpressionVisitor().ToQuery(nestedExpand.Body);

            if (!string.IsNullOrEmpty(_odataOptionNestedBase?.Query))
            {
                _stringBuilder.Append($"({_odataOptionNestedBase.Query}),{query}");
            }
            else
            {
                _stringBuilder.Append(query);
            }

            _odataOptionNestedBase = new ODataQueryExpand<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataOptionNestedBase as ODataQueryExpand<TNestedEntity>;
        }
    }
}
