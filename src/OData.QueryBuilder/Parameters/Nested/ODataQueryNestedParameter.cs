﻿using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters.Nested
{
    public class ODataQueryNestedParameter<TEntity> : ODataQueryNestedParameterBase, IODataQueryNestedParameter<TEntity>
    {
        public ODataQueryNestedParameter()
            : base(new StringBuilder())
        {
        }

        public IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            var query = entityNestedExpand.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterExpand}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterExpand}{ODataQuerySeparators.QueryStringEqualSign}{builder.Query}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter)
        {
            var query = entityNestedFilter.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterFilter}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy)
        {
            var query = entityNestedOrderBy.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterOrderBy}{ODataQuerySeparators.QueryStringEqualSign}{query} {ODataQuerySorts.QuerySortAsc}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending)
        {
            var query = entityNestedOrderByDescending.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterOrderBy}{ODataQuerySeparators.QueryStringEqualSign}{query} {ODataQuerySorts.QuerySortDesc}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var query = entityNestedSelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterSelect}{ODataQuerySeparators.QueryStringEqualSign}{query}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }

        public IODataQueryNestedParameter<TEntity> Top(int value)
        {
            _queryBuilder.Append($"{ODataQueryParameters.QueryParameterTop}{ODataQuerySeparators.QueryStringEqualSign}{value}{ODataQuerySeparators.QueryStringNestedSeparator}");

            return this;
        }
    }
}
