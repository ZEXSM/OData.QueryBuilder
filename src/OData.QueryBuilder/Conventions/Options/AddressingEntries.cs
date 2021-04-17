using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    internal class AddressingEntries<TEntity> : IAddressingEntries<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;

        public AddressingEntries(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IAddressingEntriesKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.Begin}");

            return new AddressingEntriesKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IAddressingEntriesKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){QuerySeparators.Begin}");

            return new AddressingEntriesKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IAddressingEntriesCollection<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.Begin);

            return new AddressingEntriesCollection<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
