using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;

namespace OData.QueryBuilder.Conventions.AddressingEntities
{
    internal class AddressingEntries<TEntity> : ODataQueryCollection<TEntity>, IAddressingEntries<TEntity>
    {
        public AddressingEntries(QBuilder queryBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(queryBuilder, odataQueryBuilderOptions)
        {
        }

        public IODataQueryKey<TEntity> ByKey(params int[] keys)
        {
            var key = keys.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{QuerySeparators.LeftBracket}{key}{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return this;
        }

        public IODataQueryKey<TEntity> ByKey(params string[] keys)
        {
            var key = keys.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{QuerySeparators.LeftBracket}{key}{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return this;
        }

        public IODataQueryKey<TEntity> ByKey(params Guid[] keys)
        {
            var key = keys.ToValue(_odataQueryBuilderOptions);
            _queryBuilder.Append($"{QuerySeparators.LeftBracket}{key}{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return this;
        }

        public IODataQueryCollection<TEntity> ByList()
        {
            _queryBuilder.Append(QuerySeparators.Begin);

            return this;
        }
    }
}
