using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OData.QueryBuilder
{
    public class ODataQueryBuilderList<TEntity> : IODataQueryBuilderList<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryBuilderList(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryBuilderList<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> queryFilter)
        {
            var queryFilterString = BuildExpression(queryFilter.Body, string.Empty);
            _queryBuilder.Append($"$filter={queryFilterString}&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> Expand(Expression<Func<TEntity, object>> queryExpand)
        {
            string[] expandNames = default(string[]);

            switch (queryExpand.Body)
            {
                case MemberExpression memberExpression:
                    expandNames = new string[1];

                    expandNames[0] = memberExpression.Member.Name;

                    break;
                case NewExpression newExpression:
                    expandNames = new string[newExpression.Members.Count];

                    for (var i = 0; i < newExpression.Members.Count; i++)
                    {
                        expandNames[i] = newExpression.Members[i].Name;
                    }

                    break;
                default:
                    throw new NotSupportedException($"Выражение typeof {queryExpand.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$expand={string.Join(",", expandNames)}&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> Select(Expression<Func<TEntity, object>> querySelect)
        {
            string[] selectNames = default(string[]);

            switch (querySelect.Body)
            {
                case UnaryExpression unaryExpression:
                    selectNames = new string[1];

                    selectNames[0] = ((MemberExpression)unaryExpression.Operand).Member.Name;

                    break;
                case MemberExpression memberExpression:
                    selectNames = new string[1];

                    selectNames[0] = memberExpression.Member.Name;

                    break;
                case NewExpression newExpression:
                    selectNames = new string[newExpression.Members.Count];

                    for (var i = 0; i < newExpression.Members.Count; i++)
                    {
                        selectNames[i] = newExpression.Members[i].Name;
                    }

                    break;
                default:
                    throw new NotSupportedException($"Выражение typeof {querySelect.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$select={string.Join(",", selectNames)}&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> OrderBy(Expression<Func<TEntity, object>> queryOrderBy)
        {
            string[] orderByNames = default(string[]);

            switch (queryOrderBy.Body)
            {
                case UnaryExpression unaryExpression:
                    orderByNames = new string[1];

                    orderByNames[0] = ((MemberExpression)unaryExpression.Operand).Member.Name;

                    break;
                case NewExpression newExpression:
                    orderByNames = new string[newExpression.Members.Count];

                    for (var i = 0; i < newExpression.Members.Count; i++)
                    {
                        orderByNames[i] = newExpression.Members[i].Name;
                    }

                    break;
                default:
                    throw new NotSupportedException($"Выражение typeof {queryOrderBy.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$orderby={string.Join(",", orderByNames)} asc&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> queryOrderByDescending)
        {
            string[] orderByDescendingNames = default(string[]);

            switch (queryOrderByDescending.Body)
            {
                case UnaryExpression unaryExpression:
                    orderByDescendingNames = new string[1];

                    orderByDescendingNames[0] = ((MemberExpression)unaryExpression.Operand).Member.Name;

                    break;
                case NewExpression newExpression:
                    orderByDescendingNames = new string[newExpression.Members.Count];

                    for (var i = 0; i < newExpression.Members.Count; i++)
                    {
                        orderByDescendingNames[i] = newExpression.Members[i].Name;
                    }

                    break;
                default:
                    throw new NotSupportedException($"Выражение typeof {queryOrderByDescending.Body.GetType().Name} не поддерживается.");
            }

            _queryBuilder.Append($"$orderby={string.Join(",", orderByDescendingNames)} desc&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> Skip(int number)
        {
            _queryBuilder.Append($"$skip={number}&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> Top(int number)
        {
            _queryBuilder.Append($"$top={number}&");

            return this;
        }

        public IODataQueryBuilderList<TEntity> Count()
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
