using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Visitors
{
    internal class QueryExpressionVisitor
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly ValueExpression _valueExpression;

        private bool _useParenthesis;
        private ExpressionType? _expressionType;

        public QueryExpressionVisitor(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _valueExpression = new ValueExpression();
        }

        public virtual string VisitExpression(Expression expression) => expression switch
        {
            BinaryExpression binaryExpression => VisitBinaryExpression(binaryExpression),
            MemberExpression memberExpression => VisitMemberExpression(memberExpression),
            ConstantExpression constantExpression => VisitConstantExpression(constantExpression),
            MethodCallExpression methodCallExpression => VisitMethodCallExpression(methodCallExpression),
            NewExpression newExpression => VisitNewExpression(newExpression),
            UnaryExpression unaryExpression => VisitUnaryExpression(unaryExpression),
            LambdaExpression lambdaExpression => VisitLambdaExpression(lambdaExpression),
            ParameterExpression parameterExpression => VisitParameterExpression(parameterExpression),
            _ => default,
        };

        protected virtual string VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            var hasParenthesis = _useParenthesis && HasParenthesis(binaryExpression.NodeType);

            var left = VisitExpression(binaryExpression.Left);
            var right = VisitExpression(binaryExpression.Right);

            if (string.IsNullOrEmpty(left))
            {
                return right;
            }

            if (string.IsNullOrEmpty(right))
            {
                return left;
            }

            return hasParenthesis ?
                $"({left} {binaryExpression.NodeType.ToODataQueryOperator()} {right})"
                :
                $"{left} {binaryExpression.NodeType.ToODataQueryOperator()} {right}";
        }

        protected virtual string VisitMemberExpression(MemberExpression memberExpression) =>
            IsResourceOfMemberExpression(memberExpression) ?
                CreateResourcePath(memberExpression) : _valueExpression.GetValue(memberExpression).ObjectToString();

        protected virtual string VisitConstantExpression(ConstantExpression constantExpression) =>
                constantExpression.Value.ObjectToString();

        protected virtual string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataOperator.In):
                    var in0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var in1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]).ObjectToString();

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
                    var all0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var all1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{all0}/{nameof(IODataOperator.All).ToLowerInvariant()}({all1})";
                case nameof(IODataOperator.Any):
                    var any0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var any1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{any0}/{nameof(IODataOperator.Any).ToLowerInvariant()}({any1})";
                case nameof(IODataFunction.Date):
                    var date0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.Date).ToLowerInvariant()}({date0})";
                case nameof(IODataFunction.SubstringOf):
                    var substringOf0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ObjectToString();
                    var substringOf1 = VisitExpression(methodCallExpression.Arguments[1]);

                    if (substringOf0.IsNullOrQuotes())
                    {
                        if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                        {
                            throw new ArgumentException("Value is empty or null");
                        }

                        return default;
                    }

                    return $"{nameof(IODataFunction.SubstringOf).ToLowerInvariant()}({substringOf0},{substringOf1})";
                case nameof(IODataFunction.Contains):
                    var contains0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var contains1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]).ObjectToString();

                    if (contains1.IsNullOrQuotes())
                    {
                        if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                        {
                            throw new ArgumentException("Value is empty or null");
                        }

                        return default;
                    }

                    return $"{nameof(IODataFunction.Contains).ToLowerInvariant()}({contains0},{contains1})";
                case nameof(IODataFunction.Concat):
                    var concat0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var concat1 = VisitExpression(methodCallExpression.Arguments[1]);

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
                    var toUpper0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.ToUpper).ToLowerInvariant()}({toUpper0})";
                case nameof(IODataStringAndCollectionFunction.ToLower):
                    var toLower0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.ToLower).ToLowerInvariant()}({toLower0})";
                case nameof(IConvertFunction.ConvertEnumToString):
                    return $"'{_valueExpression.GetValue(methodCallExpression.Arguments[0])}'";
                case nameof(IConvertFunction.ConvertDateTimeToString):
                    var dateTime = (DateTime)_valueExpression.GetValue(methodCallExpression.Arguments[0]);

                    return dateTime.ToString((string)_valueExpression.GetValue(methodCallExpression.Arguments[1]));
                case nameof(IConvertFunction.ConvertDateTimeOffsetToString):
                    var dateTimeOffset = (DateTimeOffset)_valueExpression.GetValue(methodCallExpression.Arguments[0]);

                    return dateTimeOffset.ToString((string)_valueExpression.GetValue(methodCallExpression.Arguments[1]));
                case nameof(IReplaceFunction.ReplaceCharacters):
                    var @symbol0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ObjectToString();
                    var @symbol1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]);

                    if (@symbol1 == default)
                    {
                        throw new ArgumentException("KeyValuePairs is null");
                    }

                    return @symbol0.ReplaceWithStringBuilder(@symbol1 as IDictionary<string, string>);
                case nameof(ToString):
                    return _valueExpression.GetValue(methodCallExpression.Object).ToString().ObjectToString();
                default:
                    return default;
            }
        }

        protected virtual string VisitNewExpression(NewExpression newExpression)
        {
            if (newExpression.Members == default)
            {
                var arguments = new object[newExpression.Arguments.Count];

                for (var i = 0; i < newExpression.Arguments.Count; i++)
                {
                    arguments[i] = _valueExpression.GetValue(newExpression.Arguments[i]);
                }

                if (newExpression.Type == typeof(DateTime) || newExpression.Type == typeof(DateTimeOffset))
                {
                    return newExpression.Constructor.Invoke(arguments).ObjectToString();
                }

                return default;
            }

            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(QuerySeparators.CommaString, names);
        }

        protected virtual string VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            var odataOperator = unaryExpression.NodeType.ToODataQueryOperator();
            var whitespace = odataOperator != default ? " " : default;

            return $"{odataOperator}{whitespace}{VisitExpression(unaryExpression.Operand)}";
        }

        protected virtual string VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            var parameterName = lambdaExpression.Parameters[0].Name;
            var filter = new QueryLambdaExpressionVisitor(_odataQueryBuilderOptions).ToString(lambdaExpression.Body);

            return $"{parameterName}:{filter}";
        }

        protected virtual string VisitParameterExpression(ParameterExpression parameterExpression) =>
            default;

        protected bool IsResourceOfMemberExpression(MemberExpression memberExpression) => memberExpression.Expression switch
        {
            ParameterExpression pe => pe.Type.GetProperty(memberExpression.Member.Name, memberExpression.Type) != default,
            MemberExpression me => IsResourceOfMemberExpression(me),
            _ => false,
        };

        protected string CreateResourcePath(MemberExpression memberExpression)
        {
            var name = VisitExpression(memberExpression.Expression);

            if (string.IsNullOrEmpty(name))
            {
                return memberExpression.Member.Name;
            }

            return memberExpression.Member.DeclaringType.IsNullableType() ?
                name : $"{name}/{memberExpression.Member.Name}";
        }

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

        public virtual string ToString(Expression expression, bool useParenthesis = false)
        {
            _useParenthesis = useParenthesis;

            return VisitExpression(expression);
        }
    }
}
