using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Resources;
using OData.QueryBuilder.Options;
using System;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : ODataQueryResource<TResource>
    {
        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base($"{baseUrl.OriginalString.TrimEnd(QuerySeparators.Slash)}{QuerySeparators.Slash}",
                  odataQueryBuilderOptions ?? new ODataQueryBuilderOptions())
        {
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base($"{baseUrl.TrimEnd(QuerySeparators.Slash)}{QuerySeparators.Slash}",
                  odataQueryBuilderOptions ?? new ODataQueryBuilderOptions())
        {
        }
    }
}