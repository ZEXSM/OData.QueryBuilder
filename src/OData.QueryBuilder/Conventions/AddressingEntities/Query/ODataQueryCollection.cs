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

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQueryCollection<TEntity> : ODataQueryKey<TEntity>, IODataQueryCollection<TEntity>
    {
        private bool _hasManyFilters;

        public ODataQueryCollection(
            QBuilder queryBuilder,
            ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(queryBuilder, odataQueryBuilderOptions)
        {
            _hasManyFilters = false;
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToString(filter, useParenthesis);

            return Filter(query);
        }

        public new IODataQueryCollection<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            base.Expand(expand);

            return this;
        }

        public new IODataQueryCollection<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            base.Expand(expandNested);

            return this;
        }

        public new IODataQueryCollection<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            base.Select(select);

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderBy);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderBy);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToString(orderByDescending);

            _queryBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Skip(int value)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Top(int value)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Count(bool value = true)
        {
            var query = value.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        private IODataQueryCollection<TEntity> Filter(string query)
        {
            if (_hasManyFilters)
            {
                _queryBuilder.Merge(ODataOptionNames.Filter, QuerySeparators.Main, $" {ODataLogicalOperations.And} {query}");
            }
            else
            {
                _queryBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasManyFilters = true;

            return this;
        }
    }
}
