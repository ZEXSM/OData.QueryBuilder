using OData.QueryBuilder.Conventions.Constants;
using System.Text;

namespace OData.QueryBuilder
{
    public class ODataQueryNested
    {
        protected readonly StringBuilder _stringBuilder;

        public ODataQueryNested(StringBuilder queryBuilder) =>
            _stringBuilder = queryBuilder;

        public string Query => _stringBuilder.ToString().Trim(QuerySeparators.NestedChar);
    }
}
