using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System;

namespace OData.QueryBuilder.Builders
{
    public abstract class AbstractODataQueryBuilder
    {
        protected readonly string _baseUrl;
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;

        public AbstractODataQueryBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = string.Empty;
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public AbstractODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = !string.IsNullOrEmpty(baseUrl) ?
                $"{baseUrl.TrimEnd(QuerySeparators.Slash)}{QuerySeparators.Slash}"
                :
                throw new ArgumentException($"{nameof(baseUrl)} is null");
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public AbstractODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : this(baseUrl?.OriginalString, odataQueryBuilderOptions)
        {
        }
    }
}
