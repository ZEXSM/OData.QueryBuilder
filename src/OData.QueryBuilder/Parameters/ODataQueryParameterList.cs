using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Functions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterList<TEntity> : IODataQueryParameterList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterList(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterList<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> queryFilter)
        {
            var odataQueryFilter = queryFilter.Body.ToODataQuery(string.Empty);
            _queryBuilder.Append($"$filter={odataQueryFilter}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> queryExpand)
        {
            var expandNames = default(string);

            switch (queryExpand.Body)
            {
                case MemberExpression memberExpression:
                    expandNames = memberExpression.ToODataQuery();
                    break;

                case NewExpression newExpression:
                    expandNames = newExpression.ToODataQuery();
                    break;

                default:
                    throw new NotSupportedException($"Выражение {queryExpand.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$expand={expandNames}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Expand(Expression<Func<IODataQueryNestedParameter<TEntity>, TEntity, object>> entityExpand)
        {
            throw new NotImplementedException();
        }

        public IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> querySelect)
        {
            var selectNames = default(string);

            switch (querySelect.Body)
            {
                case UnaryExpression unaryExpression:
                    selectNames = unaryExpression.ToODataQuery();
                    break;

                case MemberExpression memberExpression:
                    selectNames = memberExpression.ToODataQuery();
                    break;

                case NewExpression newExpression:
                    selectNames = newExpression.ToODataQuery();
                    break;

                default:
                    throw new NotSupportedException($"Выражение {querySelect.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$select={selectNames}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> queryOrderBy)
        {
            var orderByNames = default(string);

            switch (queryOrderBy.Body)
            {
                case UnaryExpression unaryExpression:
                    orderByNames = unaryExpression.ToODataQuery();
                    break;

                case NewExpression newExpression:
                    orderByNames = newExpression.ToODataQuery();
                    break;

                default:
                    throw new NotSupportedException($"Выражение {queryOrderBy.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$orderby={orderByNames} asc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> queryOrderByDescending)
        {
            var orderByDescendingNames = default(string);

            switch (queryOrderByDescending.Body)
            {
                case UnaryExpression unaryExpression:
                    orderByDescendingNames = unaryExpression.ToODataQuery();
                    break;

                case NewExpression newExpression:
                    orderByDescendingNames = newExpression.ToODataQuery();
                    break;

                default:
                    throw new NotSupportedException($"Выражение {queryOrderByDescending.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$orderby={orderByDescendingNames} desc&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Skip(int number)
        {
            _queryBuilder.Append($"$skip={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Top(int number)
        {
            _queryBuilder.Append($"$top={number}&");

            return this;
        }

        public IODataQueryParameterList<TEntity> Count()
        {
            _queryBuilder.Append("$count=true&");

            return this;
        }

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
