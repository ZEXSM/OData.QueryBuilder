using OData.QueryBuilder.Constants;
using System.Text;

namespace OData.QueryBuilder.Parameters.Nested
{
    public abstract class ODataQueryNestedParameterBase
    {
        protected readonly StringBuilder _stringBuilder;

        public ODataQueryNestedParameterBase(StringBuilder queryBuilder) =>
            _stringBuilder = queryBuilder;

        public string Query => _stringBuilder.ToString().Trim(ODataQuerySeparators.NestedChar);
    }
}
