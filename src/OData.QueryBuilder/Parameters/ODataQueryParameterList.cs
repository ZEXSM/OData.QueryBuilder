using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Functions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : IODataQueryParameterList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterList(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> entityFilter)
        {
            var odataQueryFilter = entityFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$filter={odataQueryFilter}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var expandNames = entityExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$expand={expandNames}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var selectNames = entitySelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={selectNames}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var orderByNames = entityOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={orderByNames} asc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var orderByDescendingNames = entityOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$orderby={orderByDescendingNames} desc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int number)
        {
            _queryBuilder.Append($"$skip={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int number)
        {
            _queryBuilder.Append($"$top={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count()
        {
            _queryBuilder.Append("$count=true&");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
