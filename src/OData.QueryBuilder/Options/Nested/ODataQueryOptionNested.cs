using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Options.Nested
{
    public class ODataQueryOptionNested<TEntity> : ODataQueryNested, IODataQueryOptionNested<TEntity>
    {
        public ODataQueryOptionNested()
            : base(new StringBuilder())
        {
        }

        public IODataQueryOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var visitor = new VisitorExpression(entityNestedExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var visitor = new VisitorExpression(entityNestedFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var visitor = new VisitorExpression(entityNestedOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var visitor = new VisitorExpression(entityNestedOrderByDescending.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var visitor = new VisitorExpression(entityNestedSelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryOptionNested<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataQueryOptions.Top}{QuerySeparators.EqualSignString}{value}{QuerySeparators.NestedString}");

            return this;
        }
    }
}
