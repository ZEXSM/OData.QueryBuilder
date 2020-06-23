using OData.QueryBuilder.Conventions.Constants;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    public class ODataOption<TEntity> : IODataOption<TEntity>
    {
        private readonly StringBuilder _stringBuilder;

        public ODataOption(string resourceUrl)
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
