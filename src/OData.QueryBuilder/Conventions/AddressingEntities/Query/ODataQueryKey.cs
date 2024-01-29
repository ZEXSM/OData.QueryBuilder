using OData.QueryBuilder.Conventions.AddressingEntities.Resources;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQueryKey<TEntity> : ODataQuery, IODataQueryKey<TEntity>
    {
        private bool _hasMultyExpands;
        private bool _hasMultyFilters;

        public ODataQueryKey(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
            _hasMultyExpands = false;
            _hasMultyFilters = false;
        }

        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource)
        {
            _stringBuilder.LastReplace(QuerySeparators.Begin, QuerySeparators.Slash);

            return new ODataResource<TEntity>(_stringBuilder, _odataQueryBuilderOptions).For<TResource>(resource);
        }

        public IODataQueryKey<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expand);

            return Expand(query);
        }

        public IODataQueryKey<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            return Expand(builder.Query);
        }

        public IODataQueryKey<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }
        
        public IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        private IODataQueryKey<TEntity> Expand<T>(T query) where T : class
        {
            if (_hasMultyExpands)
            {
                _stringBuilder.Merge(ODataOptionNames.Expand, QuerySeparators.Main, $"{QuerySeparators.Comma}{query}");
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasMultyExpands = true;

            return this;
        }
        
        private IODataQueryKey<TEntity> Filter(string query)
        {
            if (_hasMultyFilters)
            {
                _stringBuilder.Merge(ODataOptionNames.Filter, QuerySeparators.Main, $" {ODataLogicalOperations.And} {query}");
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasMultyFilters = true;

            return this;
        }
    }
}
