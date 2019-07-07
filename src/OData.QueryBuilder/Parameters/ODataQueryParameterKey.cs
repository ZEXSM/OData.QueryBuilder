using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : IODataQueryParameterKey<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryParameterKey(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> queryExpand)
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

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<IODataQueryNestedParameter<TEntity>, TEntity, object>> entityExpand)
        {
            throw new NotImplementedException();
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> querySelect)
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

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
