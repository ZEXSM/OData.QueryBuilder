using OData.QueryBuilder.Expressions;
using OData.QueryBuilder.Resourses;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Builders
{
    public class ODataQueryBuilder<TResource> : IODataQueryBuilder<TResource>
    {
        private readonly string _baseUrl;

        public ODataQueryBuilder(Uri baseUrl) =>
            _baseUrl = $"{baseUrl.OriginalString.TrimEnd('/')}/";

        public ODataQueryBuilder(string baseUrl) =>
            _baseUrl = $"{baseUrl.TrimEnd('/')}/";

        public IODataQueryResource<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();
            odataQueryExpressionVisitor.Visit(entityResource.Body);
            var odataResourceQuery = odataQueryExpressionVisitor.GetODataQuery();

            return new ODataQueryResource<TEntity>($"{_baseUrl}{odataResourceQuery}");
        }
    }
}