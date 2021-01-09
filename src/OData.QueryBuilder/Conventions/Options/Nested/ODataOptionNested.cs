using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Resources;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    internal class ODataOptionNested<TEntity> : ODataOptionNestedBase, IODataOptionNested<TEntity>
    {
        public ODataOptionNested(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new StringBuilder(), odataQueryBuilderOptions)
        {
        }

        public IODataOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(entityNestedExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Expand(Action<IODataQueryExpandNestedResource<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryExpandNestedResource<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityNestedFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityNestedOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityNestedOrderByDescending.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(entityNestedSelect.Body);

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
