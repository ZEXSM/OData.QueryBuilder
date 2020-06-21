using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters.Nested
{
    public class ODataQueryNestedParameter<TEntity> : ODataQueryNested, IODataQueryNestedParameter<TEntity>
    {
        public ODataQueryNestedParameter()
            : base(new StringBuilder())
        {
        }

        public IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var visitor = new Visitor(entityNestedExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{builder.Query}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var visitor = new Visitor(entityNestedFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Filter}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var visitor = new Visitor(entityNestedOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.OrderBy}{ODataQuerySeparators.EqualSignString}{query} {ODataQuerySorts.Asc}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var visitor = new Visitor(entityNestedOrderByDescending.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.OrderBy}{ODataQuerySeparators.EqualSignString}{query} {ODataQuerySorts.Desc}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var visitor = new Visitor(entityNestedSelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Select}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.NestedString}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataQueryParameters.Top}{ODataQuerySeparators.EqualSignString}{value}{ODataQuerySeparators.NestedString}");

            return this;
        }
    }
}
