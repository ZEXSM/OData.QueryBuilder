using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    internal class ODataOptionKey<TEntity> : ODataQuery<TEntity>, IODataOptionKey<TEntity>
    {
        private readonly VisitorExpression _visitorExpression;

        public ODataOptionKey(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions) =>
            _visitorExpression = new VisitorExpression(odataQueryBuilderOptions);

        public IODataOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = _visitorExpression.ToString(entityExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionKey<TEntity> Expand(Action<IODataQueryExpandNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryExpandNestedBuilder<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = _visitorExpression.ToString(entitySelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }
    }
}
