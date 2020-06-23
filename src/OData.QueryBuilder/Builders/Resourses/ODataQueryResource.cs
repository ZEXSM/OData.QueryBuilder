using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Options;
using System.Text;

namespace OData.QueryBuilder.Builders.Resourses
{
    public class ODataQueryResource<TEntity> : IODataQueryResource<TEntity>
    {
        private readonly StringBuilder _stringBuilder;

        public ODataQueryResource(string resourceUrl)
        {
            _stringBuilder = new StringBuilder(resourceUrl);
        }

        public IODataOptionKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.BeginString}");

            return new ODataOptionKey<TEntity>(_stringBuilder);
        }

        public IODataOptionKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){QuerySeparators.BeginString}");

            return new ODataOptionKey<TEntity>(_stringBuilder);
        }

        public IODataOptionList<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.BeginString);

            return new ODataOptionList<TEntity>(_stringBuilder);
        }
    }
}
