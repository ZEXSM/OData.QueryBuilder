using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Resources;
using System;
using System.Text;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : ODataQueryResource<TResource>
    {
        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(
                  new StringBuilder($"{baseUrl.OriginalString.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}"),
                  odataQueryBuilderOptions ?? new ODataQueryBuilderOptions())
        {
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(
                  new StringBuilder($"{baseUrl.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}"),
                  odataQueryBuilderOptions ?? new ODataQueryBuilderOptions())
        {
        }
    }
}