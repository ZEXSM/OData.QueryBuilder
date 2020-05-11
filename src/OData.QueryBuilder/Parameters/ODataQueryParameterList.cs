using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Functions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : ODataQuery<TEntity>, IODataQueryParameterList<TEntity>
    {
        public ODataQueryParameterList(StringBuilder queryBuilder) :
            base(queryBuilder)
        {
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var query = entityFilter.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, bool>> entityFilter)
        {
            var query = entityFilter.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = entityExpand.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{builder.Query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = entitySelect.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.Select}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var query = entityOrderBy.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.OrderBy}{ODataQuerySeparators.EqualSignString}{query} {ODataQuerySorts.Asc}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var query = entityOrderByDescending.Body.ToODataQuery();

            _stringBuilder.Append($"{ODataQueryParameters.OrderBy}{ODataQuerySeparators.EqualSignString}{query} {ODataQuerySorts.Desc}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataQueryParameters.Skip}{ODataQuerySeparators.EqualSignString}{value}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataQueryParameters.Top}{ODataQuerySeparators.EqualSignString}{value}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataQueryParameters.Count}{ODataQuerySeparators.EqualSignString}{value.ToString().ToLower()}{ODataQuerySeparators.MainString}");

            return this;
        }
    }
}
