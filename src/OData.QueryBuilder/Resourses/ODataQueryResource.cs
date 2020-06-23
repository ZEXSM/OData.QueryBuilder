using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Options;
using System.Text;

namespace OData.QueryBuilder.Resourses
{
    public class ODataQueryResource<TEntity> : IODataQueryResource<TEntity>
    {
        private readonly StringBuilder _stringBuilder;

        public ODataQueryResource(string resourceUrl)
        {
            _stringBuilder = new StringBuilder(resourceUrl);
        }

        public IODataQueryOptionKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.BeginString}");

            return new ODataQueryOptionKey<TEntity>(_stringBuilder);
        }

        public IODataQueryOptionKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){QuerySeparators.BeginString}");

            return new ODataQueryOptionKey<TEntity>(_stringBuilder);
        }

        public IODataQueryOptionList<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.BeginString);

            return new ODataQueryOptionList<TEntity>(_stringBuilder);
        }
    }
}
