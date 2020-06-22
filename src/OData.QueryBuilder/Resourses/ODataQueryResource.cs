using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Parameters;
using System.Text;

namespace OData.QueryBuilder.Resourses
{
    public class ODataQueryResource<TEntity> : IODataQueryResource<TEntity>
    {
        private readonly StringBuilder _stringBuilder;
        private readonly ODataVersion _odataVersion;

        public ODataQueryResource(string resourceUrl, ODataVersion odataVersion)
        {
            _stringBuilder = new StringBuilder(resourceUrl);
            _odataVersion = odataVersion;
        }

        public IODataQueryParameterKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){ODataQuerySeparators.BeginString}");

            return new ODataQueryParameterKey<TEntity>(_stringBuilder);
        }

        public IODataQueryParameterKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){ODataQuerySeparators.BeginString}");

            return new ODataQueryParameterKey<TEntity>(_stringBuilder);
        }

        public IODataQueryParameterList<TEntity> ByList()
        {
            _stringBuilder.Append(ODataQuerySeparators.BeginString);

            return new ODataQueryParameterList<TEntity>(_stringBuilder);
        }
    }
}
