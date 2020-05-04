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

            _queryBuilder.Append($"{Contants.QueryParameterFilter}{Contants.QueryStringEqualSign}{odataFilterQuery}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityExpand.Body);

            var odataExpandQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterExpand}{Contants.QueryStringEqualSign}{odataExpandQuery}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            entityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"{Contants.QueryParameterExpand}{Contants.QueryStringEqualSign}{odataQueryNestedBuilder.Query}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entitySelect.Body);

            var odataSelectQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterSelect}{Contants.QueryStringEqualSign}{odataSelectQuery}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityOrderBy.Body);

            var odataOrderByQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterOrderBy}{Contants.QueryStringEqualSign}{odataOrderByQuery} {Contants.QuerySortAsc}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityOrderByDescending.Body);

            var odataOrderByDescendingQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Contants.QueryParameterOrderBy}{Contants.QueryStringEqualSign}{odataOrderByDescendingQuery} {Contants.QuerySortDesc}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int value)
        {
            _queryBuilder.Append($"{Contants.QueryParameterSkip}{Contants.QueryStringEqualSign}{value}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{Contants.QueryParameterTop}{Contants.QueryStringEqualSign}{value}{Contants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count(bool value = true)
        {
            _queryBuilder.Append($"{Contants.QueryParameterCount}{Contants.QueryStringEqualSign}{value.ToString().ToLower()}{Contants.QueryStringSeparator}");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd(Contants.QueryCharSeparator));

        public Dictionary<string, string> ToDictionary()
        {
            var odataOperators = _queryBuilder.ToString()
                .Split(new char[2] { Contants.QueryCharBegin, Contants.QueryCharSeparator }, StringSplitOptions.RemoveEmptyEntries);

            var dictionary = new Dictionary<string, string>(odataOperators.Length - 1);

            for (var step = 1; step < odataOperators.Length; step++)
            {
                var odataOperator = odataOperators[step].Split(Contants.QueryCharEqualSign);

                dictionary.Add(odataOperator[0], odataOperator[1]);
            }

            return dictionary;
        }
    }
}
