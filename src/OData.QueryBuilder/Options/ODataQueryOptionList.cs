using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Functions;
using OData.QueryBuilder.Operators;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Options
{
    public class ODataQueryOptionList<TEntity> : ODataQuery<TEntity>, IODataQueryOptionList<TEntity>
    {
        public ODataQueryOptionList(StringBuilder stringBuilder)
            : base(stringBuilder)
        {
        }

        public IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Filter(Expression<Func<TEntity, IODataQueryFunction, IODataQueryOperator, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new VisitorExpression(entityExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();
            entityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryOptions.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new VisitorExpression(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var visitor = new VisitorExpression(entityOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var visitor = new VisitorExpression(entityOrderByDescending.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryOptions.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataQueryOptions.Skip}{QuerySeparators.EqualSignString}{value}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataQueryOptions.Top}{QuerySeparators.EqualSignString}{value}{QuerySeparators.MainString}");

            return this;
        }

        public IODataQueryOptionList<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataQueryOptions.Count}{QuerySeparators.EqualSignString}{value.ToString().ToLower()}{QuerySeparators.MainString}");

            return this;
        }
    }
}
