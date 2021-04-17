using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Conventions.Resources;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.Options
{
    internal class AddressingEntriesCollection<TEntity> : ODataQuery<TEntity>, IAddressingEntriesCollection<TEntity>
    {
        public AddressingEntriesCollection(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
        }

        public IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(entityFilter.Body, useParenthesis);

            _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(entityExpand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryExpandResource<TEntity>(_odataQueryBuilderOptions);

            entityExpandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.Query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(entitySelect.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> entityOrderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityOrderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(entityOrderByDescending.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IAddressingEntriesCollection<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}{QuerySeparators.Main}");

            return this;
        }
    }
}
