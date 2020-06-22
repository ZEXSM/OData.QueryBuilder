using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.V4.Constants;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.V4.Options
{
    public class ODataQueryOptionKey<TEntity> : ODataQuery<TEntity>, IODataQueryOptionKey<TEntity>
    {
        public ODataQueryOptionKey(StringBuilder stringBuilder)
            : base(stringBuilder)
        {
        }

        public IODataQueryOptionKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new VisitorExpression(entityExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new VisitorExpression(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }
    }
}
