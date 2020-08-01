using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    internal class ODataOption<TEntity> : IODataOption<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;

        public ODataOption(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataOptionKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.BeginString}");

            return new ODataOptionKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataOptionKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){QuerySeparators.BeginString}");

            return new ODataOptionKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataOptionList<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.BeginString);

            return new ODataOptionList<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
