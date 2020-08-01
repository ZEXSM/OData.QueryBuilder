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
        private readonly StringBuilder _stringBuilder;

        public ODataQueryResource(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _visitorExpression = new VisitorExpression(odataQueryBuilderOptions);
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _stringBuilder = stringBuilder;
        }

        public IODataOption<TEntity> For<TEntity>(Expression<Func<TResource, object>> entityResource)
        {
            var query = _visitorExpression.ToString(entityResource.Body);

            _stringBuilder.Append(query);

            return new ODataOption<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
