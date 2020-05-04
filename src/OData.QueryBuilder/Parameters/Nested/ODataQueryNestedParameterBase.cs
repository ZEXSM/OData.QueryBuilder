using System.Text;

namespace OData.QueryBuilder.Parameters.Nested
{
    public abstract class ODataQueryNestedParameterBase
    {
        protected readonly StringBuilder _queryBuilder;

        public ODataQueryNestedParameterBase() => _queryBuilder = new StringBuilder();

        public string Query => _queryBuilder.ToString().Trim(Contants.QueryCharNestedSeparator);
    }
}
