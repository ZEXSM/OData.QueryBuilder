using OData.QueryBuilder.Conventions.AddressingEntities;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Builders
{
    public sealed class ODataQueryBuilder<TResource>
    {
        private readonly string _baseUrl;
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;

        public ODataQueryBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = string.Empty;
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = !string.IsNullOrEmpty(baseUrl) ?
                $"{baseUrl.TrimEnd(QuerySeparators.Slash)}{QuerySeparators.Slash}"
                :
                throw new ArgumentException($"{nameof(baseUrl)} is null");
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : this(baseUrl?.OriginalString, odataQueryBuilderOptions)
        {
        }

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource) =>
           new ODataResource<TResource>(new StringBuilder(_baseUrl), _odataQueryBuilderOptions)
                .For<TEntity>(resource);
    }
}