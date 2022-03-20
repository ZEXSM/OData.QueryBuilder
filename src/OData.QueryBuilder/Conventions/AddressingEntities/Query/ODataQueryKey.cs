using OData.QueryBuilder.Conventions.AddressingEntities.Resources;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQueryKey<TEntity> : ODataQuery, IODataQueryKey<TEntity>
    {
        private bool _hasMultyExpands;

        public ODataQueryKey(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
            _hasMultyExpands = false;
        }

        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource)
        {
            _stringBuilder.LastReplace(QuerySeparators.Begin, QuerySeparators.Slash);

            return new ODataResource<TEntity>(_stringBuilder, _odataQueryBuilderOptions).For<TResource>(resource);
        }

        public IODataQueryKey<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expand.Body);

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
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
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
    }
}
