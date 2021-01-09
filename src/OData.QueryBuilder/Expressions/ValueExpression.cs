using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Expressions
{
    internal class ValueExpression
    {
        public virtual object GetValue(Expression expression) => expression switch
        {
            MemberExpression memberExpression => GetValueOfMemberExpression(memberExpression),
            ConstantExpression constantExpression => GetValueOfConstantExpression(constantExpression),
            ListInitExpression listInitExpression => GetValueOfListInitExpression(listInitExpression),
            NewArrayExpression newArrayExpression => GetValueOfNewArrayExpression(newArrayExpression),
            MethodCallExpression methodCallExpression => GetValueOfMethodCallExpression(methodCallExpression),
            _ => default,
        };

        protected virtual object GetValueOfConstantExpression(ConstantExpression constantExpression) =>
            constantExpression.Value;

        protected virtual object GetValueOfMemberExpression(MemberExpression expression) => expression.Expression switch
        {
            ConstantExpression constantExpression => expression.Member.GetValue(constantExpression.Value),
            MemberExpression memberExpression => expression.Member.GetValue(GetValueOfMemberExpression(memberExpression)),
            _ => expression.Member.GetValue(),
        };

        protected virtual object GetValueOfListInitExpression(ListInitExpression listInitExpression)
        {
            var arguments = new object[listInitExpression.NewExpression.Arguments.Count];

            for (var i = 0; i < listInitExpression.NewExpression.Arguments.Count; i++)
            {
                arguments[i] = GetValue(listInitExpression.NewExpression.Arguments[i]);
            }

            var listInit = listInitExpression.NewExpression.Constructor.Invoke(arguments);

            foreach (var elementInit in listInitExpression.Initializers)
            {
                var parameters = new object[elementInit.Arguments.Count];

                for (var index = 0; index < elementInit.Arguments.Count; index++)
                {
                    parameters[index] = GetValue(elementInit.Arguments[index]);
                }

                listInit.GetType().GetMethod(nameof(List<ListInitExpression>.Add)).Invoke(listInit, parameters);
            }

            return listInit;
        }

        protected virtual object GetValueOfNewArrayExpression(NewArrayExpression newArrayExpression)
        {
            var array = Array.CreateInstance(newArrayExpression.Type.GetElementType(), newArrayExpression.Expressions.Count);

            for (var i = 0; i < newArrayExpression.Expressions.Count; i++)
            {
                array.SetValue(GetValue(newArrayExpression.Expressions[i]), i);
            }

            return array;
        }

        protected virtual object GetValueOfMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IReplaceFunction.ReplaceCharacters):
                    var @symbol0 = GetValue(methodCallExpression.Arguments[0]) as IEnumerable<string>;
                    var @symbol1 = GetValue(methodCallExpression.Arguments[1]) as IDictionary<string, string>;

                    if (@symbol1 == default)
                    {
                        throw new ArgumentException("KeyValuePairs is null");
                    }

                    return @symbol0?.ReplaceWithStringBuilder(@symbol1);
                default:
                    return default;
            }
        }
    }
}
