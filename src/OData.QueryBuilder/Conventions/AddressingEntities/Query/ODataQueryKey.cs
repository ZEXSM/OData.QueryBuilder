using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQueryKey<TEntity> : ODataQuery, IODataQueryKey<TEntity>
    {
        private bool _hasManyExpands;

        public ODataQueryKey(
            QBuilder queryBuilder,
            ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(queryBuilder, odataQueryBuilderOptions)
        {
            _hasManyExpands = false;
        }

        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource)
        {
            _queryBuilder.LastReplace(QuerySeparators.Begin, QuerySeparators.Slash);

            return new ODataResource<TEntity>(_queryBuilder, _odataQueryBuilderOptions).For<TResource>(resource);
        }

        public IODataQueryKey<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToString(expand);

            return Expand(query);
        }

        public IODataQueryKey<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            return Expand(builder.QueryBuilder);
        }

        public IODataQueryKey<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToString(select);

            _queryBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        private IODataQueryKey<TEntity> Expand<T>(T query) where T : class
        {
            if (_hasManyExpands)
            {
                _queryBuilder.Merge(ODataOptionNames.Expand, QuerySeparators.Main, $"{QuerySeparators.Comma}{query}");
            }
            else
            {
                _queryBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasManyExpands = true;

            return this;
        }
    }
}
