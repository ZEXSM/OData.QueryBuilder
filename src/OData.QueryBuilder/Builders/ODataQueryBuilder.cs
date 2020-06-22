using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Resourses;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : IODataQueryBuilder<TResource>
    {
        private readonly string _baseUrl;
        private readonly ODataVersion _odataVersion;

        public ODataQueryBuilder(Uri baseUrl, ODataVersion odataVersion = ODataVersion.V4)
        {
            _baseUrl = $"{baseUrl.OriginalString.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}";
            _odataVersion = odataVersion;
        }

        public ODataQueryBuilder(string baseUrl, ODataVersion odataVersion = ODataVersion.V4)
        {
            _baseUrl = $"{baseUrl.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}";
            _odataVersion = odataVersion;
        }

        public IODataQueryResource<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var visitor = new VisitorExpression(entityResource.Body);
            var query = visitor.ToString();

            return new ODataQueryResource<TEntity>($"{_baseUrl}{query}", _odataVersion);
        }
    }
}