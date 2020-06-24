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
        internal readonly VisitorExpression _visitorExpression;

        public ODataOptionList(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions) =>
            _visitorExpression = new VisitorExpression();

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var query = _visitorExpression.ToString(entityFilter.Body);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter)
        {
            var query = _visitorExpression.ToString(entityFilter.Body);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter)
        {
            var query = _visitorExpression.ToString(entityFilter.Body);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = _visitorExpression.ToString(entityExpand.Body);

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
            var query = _visitorExpression.ToString(entitySelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSignString}{query}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var query = _visitorExpression.ToString(entityOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSignString}{query} {QuerySorts.Asc}{QuerySeparators.MainString}");

            return this;
        }

        public IODataOptionList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var query = _visitorExpression.ToString(entityOrderByDescending.Body);

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
