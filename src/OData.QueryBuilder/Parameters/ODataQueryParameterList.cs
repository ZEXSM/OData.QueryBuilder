using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.ExpressionVisitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : IODataQueryParameterList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterList(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter)
        {
            var visitor = new FilterODataExpressionVisitor();

            visitor.Visit(entityFilter.Body);

            _queryBuilder.Append($"{Constants.QueryParameterFilter}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new ExpandODataExpressionVisitor();

            visitor.Visit(entityExpand.Body);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(builder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{builder.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new SelectODataExpressionVisitor();

            visitor.Visit(entitySelect.Body);

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{visitor.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var visitor = new OrderByODataExpressionVisitor();

            visitor.Visit(entityOrderBy.Body);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{visitor.Query} {Constants.QuerySortAsc}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var visitor = new OrderByODataExpressionVisitor();

            visitor.Visit(entityOrderByDescending.Body);

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{visitor.Query} {Constants.QuerySortDesc}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterSkip}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Constants.QueryParameterTop}{Constants.QueryStringEqualSign}{value}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"{Constants.QueryParameterCount}{Constants.QueryStringEqualSign}{value.ToString().ToLower()}{Constants.QueryStringSeparator}");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd(Constants.QueryCharSeparator));

        public Dictionary<string, string> ToDictionary()
        {
            var odataOperators = _queryBuilder.ToString()
                .Split(new char[2] { Constants.QueryCharBegin, Constants.QueryCharSeparator }, StringSplitOptions.RemoveEmptyEntries);

            var dictionary = new Dictionary<string, string>(odataOperators.Length - 1);

            for (var step = 1; step < odataOperators.Length; step++)
            {
                var odataOperator = odataOperators[step].Split(Constants.QueryCharEqualSign);

                dictionary.Add(odataOperator[0], odataOperator[1]);
            }

            return dictionary;
        }
    }
}
