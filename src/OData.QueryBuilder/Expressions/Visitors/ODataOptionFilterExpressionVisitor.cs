using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
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

        protected override string VisitBinaryExpression(BinaryExpression binaryExpression)
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
                $"({left} {binaryExpression.NodeType.ToODataOperator()} {right})"
                :
                $"{left} {binaryExpression.NodeType.ToODataOperator()} {right}";
        }

        protected override string VisitMemberExpression(MemberExpression memberExpression) =>
            IsMemberExpressionBelongsResource(memberExpression) ? base.VisitMemberExpression(memberExpression) : _valueExpression.GetValue(memberExpression).ToQuery();

        protected override string VisitConstantExpression(ConstantExpression constantExpression) =>
            constantExpression.Value.ToQuery();

        protected override string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataOperator.In):
                    var in0 = VisitExpression(methodCallExpression.Arguments[0]);
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
                    var substringOf0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ToQuery();
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
                    var @symbol0 = _valueExpression.GetValue(methodCallExpression.Arguments[0]).ToQuery();
                    var @symbol1 = _valueExpression.GetValue(methodCallExpression.Arguments[1]);

                    if (@symbol1 == default)
                    {
                        throw new ArgumentException("KeyValuePairs is null");
                    }

                    return @symbol0.ReplaceWithStringBuilder(@symbol1 as IDictionary<string, string>);
                case nameof(string.ToString):
                    return _valueExpression.GetValue(methodCallExpression.Object).ToString().ToQuery();
                default:
                    throw new NotSupportedException($"Method {methodCallExpression.Method.Name} not supported");
            }
        }

        protected override string VisitNewExpression(NewExpression newExpression)
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
                    return newExpression.Constructor.Invoke(arguments).ToQuery();
                }

                return default;
            }

            return base.VisitNewExpression(newExpression);
        }

        protected override string VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            var parameterName = lambdaExpression.Parameters[0].Name;
            var filter = new ODataOptionFilterLambdaExpressionVisitor(_odataQueryBuilderOptions).ToQuery(lambdaExpression.Body, _useParenthesis);

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

        public virtual string ToQuery(Expression expression, bool useParenthesis = false)
        {
            _useParenthesis = useParenthesis;

            return base.ToQuery(expression);
        }
    }
}
