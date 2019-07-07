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
            var queryFilterString = BuildExpression(queryFilter.Body, string.Empty);
            _queryBuilder.Append($"$filter={queryFilterString}&");

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

        private string BuildExpression(Expression expression, string queryString)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    var leftQueryString = BuildExpression(binaryExpression.Left, queryString);
                    var rightQueryString = BuildExpression(binaryExpression.Right, queryString);

                    return $"{leftQueryString} {binaryExpression.NodeType.ToODataOperator()} {rightQueryString}";

                case MemberExpression memberExpression:
                    if (memberExpression.Expression is ConstantExpression)
                    {
                        if (memberExpression.Member is FieldInfo)
                        {
                            var valueConstantExpression = ((FieldInfo)memberExpression.Member).GetValue(((ConstantExpression)memberExpression.Expression).Value);

                            if (valueConstantExpression is IEnumerable<int>)
                            {
                                return string.Join(",", (IEnumerable<int>)valueConstantExpression);
                            }

                            if (valueConstantExpression is IEnumerable<string>)
                            {
                                return $"'{string.Join("','", (IEnumerable<string>)valueConstantExpression)}'";
                            }

                            return valueConstantExpression.ToString();
                        }
                    }

                    var parentMemberExpressionQuery = BuildExpression(memberExpression.Expression, queryString);

                    if (string.IsNullOrEmpty(parentMemberExpressionQuery))
                    {
                        return memberExpression.Member.Name;
                    }

                    return $"{parentMemberExpressionQuery}/{memberExpression.Member.Name}";

                case ConstantExpression constantExpression:
                    return constantExpression.Value?.ToString() ?? "null";

                case MethodCallExpression methodCallExpression:
                    var methodName = methodCallExpression.Method.Name;
                    var methodParameters = BuildExpression(methodCallExpression.Arguments[0], queryString);

                    if (methodName == nameof(string.Contains))
                    {

                        var valueConstantExpression = BuildExpression(methodCallExpression.Object, queryString);


                        return $"{methodParameters} in ({valueConstantExpression})";
                    }

                    return $"{methodName.ToLower()}({methodParameters})";

                default:
                    return string.Empty;
            }
        }
    }
}
