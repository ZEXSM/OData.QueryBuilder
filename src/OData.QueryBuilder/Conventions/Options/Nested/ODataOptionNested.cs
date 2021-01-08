using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Resources;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    internal class ODataOptionNested<TEntity> : ODataOptionNestedBase, IODataOptionNested<TEntity>
    {
        private readonly QueryExpressionVisitor _queryExpressionVisitor;

        public ODataOptionNested(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(new StringBuilder(), odataQueryBuilderOptions)
        {
            _queryExpressionVisitor = new QueryExpressionVisitor(odataQueryBuilderOptions);
        }

        public IODataOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var query = _queryExpressionVisitor.ToString(entityNestedExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Expand(Action<IODataQueryExpandNestedResource<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryExpandNestedResource<TEntity>(_odataQueryBuilderOptions);

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter, bool useParenthesis = false)
        {
            var query = _queryExpressionVisitor.ToString(entityNestedFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var query = _queryExpressionVisitor.ToString(entityNestedOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var query = _queryExpressionVisitor.ToString(entityNestedOrderByDescending.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var query = _queryExpressionVisitor.ToString(entityNestedSelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.NestedString}");

            return this;
        }

        public IODataOptionNested<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSignString}{value}{QuerySeparators.NestedString}");

            return this;
        }
    }
}
