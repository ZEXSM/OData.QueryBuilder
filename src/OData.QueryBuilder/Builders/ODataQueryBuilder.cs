using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Options;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : IODataQueryBuilder<TResource>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly string _baseUrl;

        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = $"{baseUrl.OriginalString.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}";
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
        {
            _baseUrl = $"{baseUrl.TrimEnd(QuerySeparators.SlashChar)}{QuerySeparators.SlashString}";
            _odataQueryBuilderOptions = odataQueryBuilderOptions ?? new ODataQueryBuilderOptions();
        }

        public IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var visitor = new VisitorExpression(entityResource.Body);
            var query = visitor.ToString();

            return new ODataOption<TEntity>($"{_baseUrl}{query}", _odataQueryBuilderOptions);
        }
    }
}