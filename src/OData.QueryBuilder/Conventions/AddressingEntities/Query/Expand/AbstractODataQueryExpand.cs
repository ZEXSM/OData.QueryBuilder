using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand
{
    internal abstract class AbstractODataQueryExpand
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly QBuilder _queryBuilder;

        public AbstractODataQueryExpand(
            QBuilder queryBuilder,
            ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _queryBuilder = queryBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public QBuilder QueryBuilder => _queryBuilder.LastRemove(QuerySeparators.Nested);
    }
}
