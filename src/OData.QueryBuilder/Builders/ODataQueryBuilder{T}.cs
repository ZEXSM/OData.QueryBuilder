using OData.QueryBuilder.Conventions.AddressingEntities;
using OData.QueryBuilder.Conventions.AddressingEntities.Resources;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Builders
{
    public sealed class ODataQueryBuilder<TResource> : AbstractODataQueryBuilder
    {
        public ODataQueryBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(odataQueryBuilderOptions)
        {
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
             : base(baseUrl, odataQueryBuilderOptions)
        {
        }

        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
             : base(baseUrl, odataQueryBuilderOptions)
        {
        }

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource) =>
           new ODataResource<TResource>(new StringBuilder(_baseUrl), _odataQueryBuilderOptions)
                .For<TEntity>(resource);
    }
}