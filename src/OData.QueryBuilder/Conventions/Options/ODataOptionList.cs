using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Options;
using OData.QueryBuilder.Visitors;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    public class ODataOptionList<TEntity> : ODataQuery<TEntity>, IODataOptionList<TEntity>
    {
        public ODataOptionList(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
        }

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter)
        {
            var visitor = new VisitorExpression(entityFilter.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new VisitorExpression(entityExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>(_odataQueryBuilderOptions);
            entityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSignString}{builder.Query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new VisitorExpression(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var visitor = new VisitorExpression(entityOrderBy.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var visitor = new VisitorExpression(entityOrderByDescending.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Desc}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSignString}{value}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSignString}{value}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSignString}{value.ToString().ToLower()}{QuerySeparators.MainString}");

            return this;
        }
    }
}
