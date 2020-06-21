using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Functions;
using OData.QueryBuilder.Operators;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : ODataQuery<TEntity>, IODataQueryParameterList<TEntity>
    {
        public ODataQueryParameterList(StringBuilder stringBuilder) :
            base(stringBuilder)
        {
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var visitor = new Visitor(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, bool>> entityFilter)
        {
            var visitor = new Visitor(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, IODataQueryOperator, bool>> entityFilter)
        {
            var visitor = new Visitor(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new Visitor(entityExpand.Body);
            var query = visitor.ToString();

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
            var visitor = new Visitor(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Select}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var visitor = new Visitor(entityOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.OrderBy}{ODataQuerySeparators.EqualSignString}{query} {ODataQuerySorts.Asc}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var visitor = new Visitor(entityOrderByDescending.Body);
            var query = visitor.ToString();

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
