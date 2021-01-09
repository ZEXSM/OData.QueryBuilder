using OData.QueryBuilder.Conventions.Options;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Resources
{
    public class ODataQueryResource<TResource> : IODataQueryResource<TResource>
    {
        private readonly ODataResourceExpressionVisitor _odataQueryResourceExpressionVisitor;
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly string _resourse;

        public ODataQueryResource(string resourse, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryResourceExpressionVisitor = new ODataResourceExpressionVisitor();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _resourse = resourse;
        }

        public IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var query = _odataQueryResourceExpressionVisitor.ToQuery(entityResource.Body);

            return new ODataOption<TEntity>(new StringBuilder($"{_resourse}{query}"), _odataQueryBuilderOptions);
        }
    }
}
