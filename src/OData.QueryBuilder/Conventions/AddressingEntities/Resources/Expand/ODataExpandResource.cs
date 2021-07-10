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
        private AbstractODataQueryExpand _odataQueryExpand;

        public StringBuilder Query => new StringBuilder($"{_stringBuilder}({_odataQueryExpand.Query})");

        public ODataExpandResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataQueryExpand<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedExpand)
        {
            var query = new ODataResourceExpressionVisitor().ToQuery(nestedExpand.Body);

            if (_odataQueryExpand?.Query != default && _odataQueryExpand?.Query.Length != 0)
            {
                _stringBuilder.Append($"({_odataQueryExpand.Query}),{query}");
            }
            else
            {
                _stringBuilder.Append(query);
            }

            _odataQueryExpand = new ODataQueryExpand<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataQueryExpand as ODataQueryExpand<TNestedEntity>;
        }
    }
}
