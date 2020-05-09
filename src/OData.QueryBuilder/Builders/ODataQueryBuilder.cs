using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Parameters;
using OData.QueryBuilder.Resourses;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : IODataQueryBuilder<TResource>
    {
        private readonly string _baseUrl;

        public ODataQueryBuilder(Uri baseUrl) =>
            _baseUrl = $"{baseUrl.OriginalString.TrimEnd(Constants.SlashCharSeparator)}{Constants.SlashStringSeparator}";

        public ODataQueryBuilder(string baseUrl) =>
            _baseUrl = $"{baseUrl.TrimEnd(Constants.SlashCharSeparator)}{Constants.SlashStringSeparator}";

        public IODataQueryResource<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource) =>
            new ODataQueryResource<TEntity>($"{_baseUrl}{entityResource.Body.ToODataQuery(string.Empty)}");
    }
}