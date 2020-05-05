using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : IODataQueryParameterKey<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterKey(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entityExpand.Body);

            var odataExpandQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataExpandQuery}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var odataQueryNestedBuilder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(odataQueryNestedBuilder);

            _queryBuilder.Append($"{Constants.QueryParameterExpand}{Constants.QueryStringEqualSign}{odataQueryNestedBuilder.Query}{Constants.QueryStringSeparator}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var odataQueryExpressionVisitor = new ODataQueryExpressionVisitor();

            odataQueryExpressionVisitor.Visit(entitySelect.Body);

            var odataSelectQuery = odataQueryExpressionVisitor.GetODataQuery();

            _queryBuilder.Append($"{Constants.QueryParameterSelect}{Constants.QueryStringEqualSign}{odataSelectQuery}{Constants.QueryStringSeparator}");

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
