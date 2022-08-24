using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal class ODataResource<TResource> : IODataResource<TResource>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;

        public ODataResource(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _stringBuilder = stringBuilder;
        }

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource), "Resource name is null");
            }

            var query = new ODataResourceExpressionVisitor().ToQuery(resource);

            _stringBuilder.Append(query);

            return new AddressingEntries<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
