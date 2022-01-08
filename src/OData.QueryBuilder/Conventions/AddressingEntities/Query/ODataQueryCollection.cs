using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQueryCollection<TEntity> : ODataQuery, IODataQueryCollection<TEntity>
    {
        private int _countFilters;
        public ODataQueryCollection(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
            _countFilters = 0;
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false) =>
            FilterByLambdaExpression(filter, useParenthesis);

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false) =>
            FilterByLambdaExpression(filter, useParenthesis);

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false) =>
            FilterByLambdaExpression(filter, useParenthesis);

        public IODataQueryCollection<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expand.Body);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{builder.Query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select.Body);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByDescending.Body);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}{QuerySeparators.Main}");

            return this;
        }

        private IODataQueryCollection<TEntity> FilterByLambdaExpression(LambdaExpression filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter.Body, useParenthesis);

            if (_countFilters > 0)
            {
                for (var i = 0; i < _stringBuilder.Length; i--)
                {
                    if (_stringBuilder[i] == '$' && _stringBuilder[i + 1] == 'f' && _stringBuilder[i + 2] == 'i')
                    {

                    }
                }
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _countFilters++;

            return this;
        }
    }
}
