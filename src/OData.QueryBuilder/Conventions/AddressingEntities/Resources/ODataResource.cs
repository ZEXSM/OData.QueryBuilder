using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    public class ODataResource<TResource> : IODataResource<TResource>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly string _resourse;

        public ODataResource(string resourse, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _resourse = resourse;
        }

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource)
        {
            var query = new ODataResourceExpressionVisitor().ToQuery(resource.Body);

            return new AddressingEntries<TEntity>(new StringBuilder($"{_resourse}{query}"), _odataQueryBuilderOptions);
        }
    }
}
