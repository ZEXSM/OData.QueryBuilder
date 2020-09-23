using OData.QueryBuilder.Conventions.Options;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Resources
{
    public class ODataQueryResource<TResource> : IODataQueryResource<TResource>
    {
        private readonly VisitorExpression _visitorExpression;
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly string _resourse;

        public ODataQueryResource(string resourse, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _visitorExpression = new VisitorExpression(odataQueryBuilderOptions);
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _resourse = resourse;
        }

        public IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var query = _visitorExpression.ToString(entityResource.Body);

            return new ODataOption<TEntity>(new StringBuilder($"{_resourse}{query}"), _odataQueryBuilderOptions);
        }
    }
}
