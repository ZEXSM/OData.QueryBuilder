using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Resources;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    internal class AddressingEntriesKey<TEntity> : ODataQuery<TEntity>, IAddressingEntriesKey<TEntity>
    {
        public AddressingEntriesKey(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
        }

        public IAddressingEntriesKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(entityExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesKey<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryExpandResource<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.Query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(entitySelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }
    }
}
