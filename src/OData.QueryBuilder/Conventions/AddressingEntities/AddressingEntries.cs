using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities
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

        public IODataQueryKey<TEntity> ByKey(int key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryKey<TEntity> ByKey(string key)
        {
            _stringBuilder.Append($"('{key}'){QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryKey<TEntity> ByKey(Guid key)
        {
            _stringBuilder.Append($"({key}){QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryCollection<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.Begin);

            return new ODataQueryCollection<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
