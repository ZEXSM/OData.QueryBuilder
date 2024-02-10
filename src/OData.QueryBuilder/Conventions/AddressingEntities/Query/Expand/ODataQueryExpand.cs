using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand
{
    internal class ODataQueryExpand<TEntity> : AbstractODataQueryExpand, IODataQueryExpand<TEntity>
    {
        private bool _hasManyFilters;

        public ODataQueryExpand(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new QBuilder(), odataQueryBuilderOptions)
        {
            _hasManyFilters = false;
        }

        public IODataQueryExpand<TEntity> Expand(Expression<Func<TEntity, object>> expandNested)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToString(expandNested);

            _queryBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            _queryBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.QueryBuilder}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filterNested, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filterNested, useParenthesis);

            return Filter(query);
        }

        public IODataQueryExpand<TEntity> OrderBy(Expression<Func<TEntity, object>> orderByNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderByNested);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderByNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderByNested);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescendingNested)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderByDescendingNested);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Select(Expression<Func<TEntity, object>> selectNested)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToString(selectNested);

            _queryBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Skip(int value)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Top(int value)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        public IODataQueryExpand<TEntity> Count(bool value = true)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");

            return this;
        }

        private IODataQueryExpand<TEntity> Filter(string query)
        {
            if (_hasManyFilters)
            {
                _queryBuilder.Merge(ODataOptionNames.Filter, QuerySeparators.Nested, $" {ODataLogicalOperations.And} {query}");
            }
            else
            {
                _queryBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Nested}");
            }

            _hasManyFilters = true;

            return this;
        }
    }
}
