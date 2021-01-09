using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    internal class ODataOptionNestedBase
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly StringBuilder _stringBuilder;

        public ODataOptionNestedBase(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public string Query => _stringBuilder.ToString().Trim(QuerySeparators.Nested);
    }
}
