﻿using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Expressions.Visitors
{
    internal class ODataOptionFilterExpressionVisitor : ODataOptionExpressionVisitor
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly ValueExpression _valueExpression;

        private bool _useParenthesis;
        private ExpressionType? _expressionType;

        public ODataOptionFilterExpressionVisitor(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base()
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _valueExpression = new ValueExpression();
        }

        protected override string VisitBinaryExpression(LambdaExpression topExpression, BinaryExpression binaryExpression)
        {
            var hasParenthesis = _useParenthesis && HasParenthesis(binaryExpression.NodeType);

            var left = VisitExpression(topExpression, binaryExpression.Left);
            var right = VisitExpression(topExpression, binaryExpression.Right);

            if (string.IsNullOrEmpty(left))
            {
                return right;
            }

            if (string.IsNullOrEmpty(right))
            {
                return left;
            }

            return hasParenthesis ?
                $"{QuerySeparators.LeftBracket}{left} {binaryExpression.NodeType.ToODataOperator()} {right}{QuerySeparators.RigthBracket}"
                :
                $"{left} {binaryExpression.NodeType.ToODataOperator()} {right}";
        }

        protected override string VisitMemberExpression(LambdaExpression topExpression, MemberExpression memberExpression) =>
            IsMemberExpressionBelongsResource(memberExpression) ? base.VisitMemberExpression(topExpression, memberExpression) : _valueExpression.GetValue(memberExpression).ToQuery();

        protected override string VisitConstantExpression(LambdaExpression topExpression, ConstantExpression constantExpression) =>
            constantExpression.Value.ToQuery();

        protected override string VisitMethodCallExpression(LambdaExpression topExpression, MethodCallExpression methodCallExpression)
        {
            var declaringType = methodCallExpression.Method.DeclaringType!;

            if (declaringType.IsAssignableFrom(typeof(IODataOperator)))
            {
                switch (methodCallExpression.Method.Name)
                {
                    case nameof(IODataOperator.In):
                        var in0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var in1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]).ToQuery();

                        if (in1.IsNullOrQuotes())
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyOperatorArgs)
                            {
                                throw new ArgumentException("Enumeration is empty or null");
                            }

                            return default;
                        }

                        return $"{in0} {nameof(IODataOperator.In).ToLowerInvariant()} ({in1})";
                    case nameof(IODataOperator.All):
                        var all0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var all1 = VisitExpression(topExpression, methodCallExpression.Arguments[1]);

                        return $"{all0}/{nameof(IODataOperator.All).ToLowerInvariant()}({all1})";
                    case nameof(IODataOperator.Any):
                        var any0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var any1 = default(string);

                        if (methodCallExpression.Arguments.Count > 1)
                        {
                            any1 = VisitExpression(topExpression, methodCallExpression.Arguments[1]);

                            if (any1.IsNullOrQuotes())
                            {
                                if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyOperatorArgs)
                                {
                                    throw new ArgumentException("Func is null");
                                }

                                return default;
                            }
                        }

                        return $"{any0}/{nameof(IODataOperator.Any).ToLowerInvariant()}({any1})";
                }
            }
            
            if (declaringType.IsAssignableFrom(typeof(IODataFunction)))
            {
                switch (methodCallExpression.Method.Name)
                {
                    case nameof(IODataFunction.Date):
                        var date0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        return $"{nameof(IODataFunction.Date).ToLowerInvariant()}({date0})";
                    case nameof(IODataFunction.SubstringOf):
                        var substringOf0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ToQuery();
                        var substringOf1 = VisitExpression(topExpression, methodCallExpression.Arguments[1]);

                        if (substringOf0.IsNullOrQuotes())
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                            {
                                throw new ArgumentException("Value is empty or null");
                            }

                            return default;
                        }

                        return
                            $"{nameof(IODataFunction.SubstringOf).ToLowerInvariant()}({substringOf0},{substringOf1})";
                    case nameof(IODataFunction.Contains):
                        var contains0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var contains1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]).ToQuery();

                        if (contains1.IsNullOrQuotes())
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                            {
                                throw new ArgumentException("Value is empty or null");
                            }

                            return default;
                        }

                        return $"{nameof(IODataFunction.Contains).ToLowerInvariant()}({contains0},{contains1})";
                    case nameof(IODataFunction.StartsWith):
                        var startsWith0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var startsWith1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]).ToQuery();

                        if (startsWith1.IsNullOrQuotes())
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                            {
                                throw new ArgumentException("Value is empty or null");
                            }

                            return default;
                        }

                        return $"{nameof(IODataFunction.StartsWith).ToLowerInvariant()}({startsWith0},{startsWith1})";
                    case nameof(IODataFunction.Concat):
                        var concat0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var concat1 = VisitExpression(topExpression, methodCallExpression.Arguments[1]);

                        if (concat0.IsNullOrQuotes() || concat1.IsNullOrQuotes())
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                            {
                                throw new ArgumentException("Value is empty or null");
                            }

                            return default;
                        }

                        return $"{nameof(IODataFunction.Concat).ToLowerInvariant()}({concat0},{concat1})";
                    case nameof(IODataStringAndCollectionFunction.ToUpper):
                        var toUpper0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        return $"{nameof(IODataFunction.ToUpper).ToLowerInvariant()}({toUpper0})";
                    case nameof(IODataStringAndCollectionFunction.ToLower):
                        var toLower0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        return $"{nameof(IODataFunction.ToLower).ToLowerInvariant()}({toLower0})";
                    case nameof(IODataStringAndCollectionFunction.IndexOf):
                        var indexOf0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var indexOf1 = VisitExpression(topExpression, methodCallExpression.Arguments[1]);

                        return $"{nameof(IODataFunction.IndexOf).ToLowerInvariant()}({indexOf0},{indexOf1})";
                    case nameof(IODataStringAndCollectionFunction.Length):
                        var length0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                        return $"{nameof(IODataStringAndCollectionFunction.Length).ToLowerInvariant()}({length0})";
                    case nameof(ICustomFunction.ConvertEnumToString):
                        return $"'{_valueExpression.GetValue(methodCallExpression.Arguments[0])}'";
                    case nameof(ICustomFunction.ConvertDateTimeToString):
                        var dateTime = (DateTime)_valueExpression.GetValue(methodCallExpression.Arguments[0]);

                        return dateTime.ToString((string)_valueExpression.GetValue(methodCallExpression.Arguments[1]));
                    case nameof(ICustomFunction.ConvertDateTimeOffsetToString):
                        var dateTimeOffset =
                            (DateTimeOffset)_valueExpression.GetValue(methodCallExpression.Arguments[0]);

                        return dateTimeOffset.ToString(
                            (string)_valueExpression.GetValue(methodCallExpression.Arguments[1]));
                    case nameof(ICustomFunction.ReplaceCharacters):
                        var @symbol0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ToQuery();
                        var @symbol1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]);

                        if (@symbol1 == default)
                        {
                            throw new ArgumentException("KeyValuePairs is null");
                        }

                        return @symbol0.ReplaceWithStringBuilder(@symbol1 as IDictionary<string, string>);
                    case nameof(ITypeFunction.Cast):
                        var cast0 = VisitExpression(topExpression, methodCallExpression.Arguments[0]);
                        var cast1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]) as string;

                        if (string.IsNullOrEmpty(cast1))
                        {
                            if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                            {
                                throw new ArgumentException("Type is empty or null");
                            }

                            return default;
                        }

                        return $"{nameof(ITypeFunction.Cast).ToLowerInvariant()}({cast0},{cast1})";
                }
            }

            if (typeof(object).IsAssignableFrom(declaringType))
            {
                switch (methodCallExpression.Method.Name)
                {
                    case nameof(object.ToString):
                        return _valueExpression.GetValue(methodCallExpression.Object).ToString().ToQuery();
                }
            }

            return base.VisitMethodCallExpression(topExpression, methodCallExpression);
        }

        protected override string VisitNewExpression(LambdaExpression topExpression, NewExpression newExpression)
        {
            if (newExpression.Members == default)
            {
                var arguments = new object[newExpression.Arguments.Count];

                for (var i = 0; i < newExpression.Arguments.Count; i++)
                {
                    arguments[i] = _valueExpression.GetValue(newExpression.Arguments[i]);
                }

                return (arguments.Length == 0 ? Activator.CreateInstance(newExpression.Type) : newExpression.Constructor.Invoke(arguments)).ToQuery();
            }

            return base.VisitNewExpression(topExpression, newExpression);
        }

        protected override string VisitLambdaExpression(LambdaExpression topExpression, LambdaExpression lambdaExpression)
        {
            var parameterName = lambdaExpression.Parameters[0].Name;
            var filter = new ODataOptionFilterLambdaExpressionVisitor(_odataQueryBuilderOptions).ToQuery(lambdaExpression, _useParenthesis);

            return $"{parameterName}:{filter}";
        }

        private bool IsMemberExpressionBelongsResource(MemberExpression memberExpression) => memberExpression.Expression switch
        {
            ParameterExpression parameterExpression => parameterExpression.Type.GetProperty(memberExpression.Member.Name, memberExpression.Type) != default,
            MemberExpression childMemberExpression => IsMemberExpressionBelongsResource(childMemberExpression),
            _ => false,
        };

        private bool HasParenthesis(ExpressionType expressionType)
        {
            var hasParenthesis = _expressionType.HasValue && expressionType switch
            {
                ExpressionType.And => true,
                ExpressionType.AndAlso => true,
                ExpressionType.Or => true,
                ExpressionType.OrElse => true,
                _ => false,
            };

            _expressionType = expressionType;

            return hasParenthesis;
        }

        public virtual string ToQuery(LambdaExpression expression, bool useParenthesis = false)
        {
            _useParenthesis = useParenthesis;

            return base.ToQuery(expression);
        }
    }
}
