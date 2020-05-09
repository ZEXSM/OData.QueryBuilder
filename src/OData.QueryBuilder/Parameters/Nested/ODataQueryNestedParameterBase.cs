using OData.QueryBuilder.Constants;
using System.Text;

namespace OData.QueryBuilder.Parameters.Nested
{
    public abstract class ODataQueryNestedParameterBase
    {
        protected readonly StringBuilder _queryBuilder;

        public ODataQueryNestedParameterBase(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public string Query => _queryBuilder.ToString().Trim(ODataQuerySeparators.QueryCharNestedSeparator);
    }
}
