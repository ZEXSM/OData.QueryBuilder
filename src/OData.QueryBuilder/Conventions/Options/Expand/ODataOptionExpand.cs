using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Conventions.Resources;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options.Expand
{
    internal class ODataOptionExpand<TEntity> : ODataOptionExpandBase, IODataOptionExpand<TEntity>
    {
        public ODataOptionExpand(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new StringBuilder(), odataQueryBuilderOptions)
        {
        }

        public IODataOptionExpand<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(entityNestedExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryExpandResource<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.Query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityNestedFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityNestedOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> entityOrderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityNestedOrderByDescending.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(entityNestedSelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataOptionExpand<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Nested}");

            return this;
        }
    }
}
