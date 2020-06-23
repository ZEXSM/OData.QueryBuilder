using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    public class ODataOptionKey<TEntity> : ODataQuery<TEntity>, IODataOptionKey<TEntity>
    {
        public ODataOptionKey(StringBuilder stringBuilder)
            : base(stringBuilder)
        {
        }

        public IODataOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new VisitorExpression(entityExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new VisitorExpression(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }
    }
}
