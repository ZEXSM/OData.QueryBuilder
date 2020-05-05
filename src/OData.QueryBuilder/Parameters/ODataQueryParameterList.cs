using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Expressions;
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
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityFilter.Body);

            var odataFilterQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterFilter}{Constants.QueryStringEqualSign}{odataFilterQuery}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityExpand.Body);

            var odataExpandQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataExpandQuery}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataQueryNestedBuilder.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entitySelect.Body);

            var odataSelectQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{odataSelectQuery}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityOrderBy.Body);

            var odataOrderByQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{odataOrderByQuery} {Constants.QuerySortAsc}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityOrderByDescending.Body);

            var odataOrderByDescendingQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterOrderBy}{Constants.QueryStringEqualSign}{odataOrderByDescendingQuery} {Constants.QuerySortDesc}{Constants.QueryStringSeparator}");

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
