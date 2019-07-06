using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder
{
    public class ODataQueryBuilderKey<TEntity> : IODataQueryBuilderKey<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryBuilderKey(StringBuilder queryBuilder) =>
            _queryBuilder = queryBuilder;

        public IODataQueryBuilderKey<TEntity> Expand(Expression<Func<TEntity, object>> queryExpand)
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

        public IODataQueryBuilderKey<TEntity> Select(Expression<Func<TEntity, object>> querySelect)
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

        public Uri ToUri() => new Uri(_queryBuilder.ToString().TrimEnd('&'));
    }
}
