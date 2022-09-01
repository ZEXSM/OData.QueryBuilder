using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand
{
    internal class ODataQueryExpand<TEntity> : AbstractODataQueryExpand, IODataQueryExpand<TEntity>
    {
        private bool _hasMultyFilters;
        public ODataQueryExpand(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new StringBuilder(), odataQueryBuilderOptions)
        {
            _hasMultyFilters = false;
        }
        public IODataQueryExpand<TEntity> Expand(Expression<Func<TEntity, object>> expandNested)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expandNested);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.Query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> OrderBy(Expression<Func<TEntity, object>> orderByNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByNested);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderByNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByNested);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescendingNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByDescendingNested);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Select(Expression<Func<TEntity, object>> selectNested)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(selectNested);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}{QuerySeparators.Nested}");

            return this;
        }

        private IODataQueryExpand<TEntity> Filter(string query)
        {
            if (_hasMultyFilters)
            {
                _stringBuilder.Merge(ODataOptionNames.Filter, QuerySeparators.Nested, $" {ODataLogicalOperations.And} {query}");
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");
            }

            _hasMultyFilters = true;

            return this;
        }
    }
}
