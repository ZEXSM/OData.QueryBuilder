using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System;

namespace OData.QueryBuilder.Builders
{
    public abstract class BaseODataQueryBuilder
    {
        protected readonly string _baseUrl;
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;

        public BaseODataQueryBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = string.Empty;
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public BaseODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = !string.IsNullOrEmpty(baseUrl) ?
                $"{baseUrl.TrimEnd(QuerySeparators.Slash)}{QuerySeparators.Slash}"
                :
                throw new ArgumentException($"{nameof(baseUrl)} is null");
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public BaseODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : this(baseUrl?.OriginalString, odataQueryBuilderOptions)
        {
        }
    }
}
