using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    public class ODataOptionNested<TEntity> : ODataQueryNested, IODataOptionNested<TEntity>
    {
        public ODataOptionNested(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new StringBuilder(), odataQueryBuilderOptions)
        {
        }

        public IODataOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var visitor = new VisitorExpression(entityNestedExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var visitor = new VisitorExpression(entityNestedFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var visitor = new VisitorExpression(entityNestedOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var visitor = new VisitorExpression(entityNestedOrderByDescending.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var visitor = new VisitorExpression(entityNestedSelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSignString}{value}{QuerySeparators.NestedString}");

            return this;
        }
    }
}
