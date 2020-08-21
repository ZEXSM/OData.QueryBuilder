using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Visitors
{
    internal class VisitorExpression
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;

        public VisitorExpression(ODataQueryBuilderOptions odataQueryBuilderOptions) =>
            _odataQueryBuilderOptions = odataQueryBuilderOptions;

        protected virtual string VisitExpression(Expression expression) => expression switch
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

            return $"{left} {binaryExpression.NodeType.ToODataQueryOperator()} {right}";
        }

        protected virtual string VisitMemberExpression(MemberExpression memberExpression) =>
            IsResourceOfMemberExpression(memberExpression) ?
                CreateResourcePath(memberExpression) : ReflectionExtensions.ConvertToString(GetValueOfMemberExpression(memberExpression));

        protected virtual string VisitConstantExpression(ConstantExpression constantExpression) =>
                ReflectionExtensions.ConvertToString(constantExpression.Value);

        protected virtual string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataOperator.In):
                    var in0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var in1 = ReflectionExtensions.ConvertToString(GetValueOfExpression(methodCallExpression.Arguments[1]));

                    if (in1.IsNullOrQuotes())
                    {
                        if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyOperatorArgs)
                        {
                            throw new ArgumentException("Enumeration is empty or null");
                        }

                        return default;
                    }

                    return $"{in0} {nameof(IODataOperator.In).ToLower()} ({in1})";
                case nameof(IODataOperator.All):
                    var all0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var all1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{all0}/{nameof(IODataOperator.All).ToLower()}({all1})";
                case nameof(IODataOperator.Any):
                    var any0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var any1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{any0}/{nameof(IODataOperator.Any).ToLower()}({any1})";
                case nameof(IODataFunction.Date):
                    var date0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.Date).ToLower()}({date0})";
                case nameof(IODataFunction.SubstringOf):
                    var substringOf0 = ReflectionExtensions.ConvertToString(GetValueOfExpression(methodCallExpression.Arguments[0]));
                    var substringOf1 = VisitExpression(methodCallExpression.Arguments[1]);

                    if (substringOf0.IsNullOrQuotes())
                    {
                        if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                        {
                            throw new ArgumentException("Value is empty or null");
                        }

                        return default;
                    }

                    return $"{nameof(IODataFunction.SubstringOf).ToLower()}({substringOf0},{substringOf1})";
                case nameof(IODataFunction.Contains):
                    var contains0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var contains1 = ReflectionExtensions.ConvertToString(GetValueOfExpression(methodCallExpression.Arguments[1]));

                    if (contains1.IsNullOrQuotes())
                    {
                        if (!_odataQueryBuilderOptions.SuppressExceptionOfNullOrEmptyFunctionArgs)
                        {
                            throw new ArgumentException("Value is empty or null");
                        }

                        return default;
                    }

                    return $"{nameof(IODataFunction.Contains).ToLower()}({contains0},{contains1})";
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

                    return $"{nameof(IODataFunction.Concat).ToLower()}({concat0},{concat1})";
                case nameof(IODataStringAndCollectionFunction.ToUpper):
                    var toUpper0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.ToUpper).ToLower()}({toUpper0})";
                case nameof(IODataStringAndCollectionFunction.ToLower):
                    var toLower0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{nameof(IODataFunction.ToLower).ToLower()}({toLower0})";
                case nameof(IConvertFunction.ConvertEnumToString):
                    return $"'{GetValueOfExpression(methodCallExpression.Arguments[0])}'";
                case nameof(IConvertFunction.ConvertDateTimeToString):
                    var dateTime = (DateTime)GetValueOfExpression(methodCallExpression.Arguments[0]);

                    return dateTime.ToString((string)GetValueOfExpression(methodCallExpression.Arguments[1]));
                case nameof(IConvertFunction.ConvertDateTimeOffsetToString):
                    var dateTimeOffset = (DateTimeOffset)GetValueOfExpression(methodCallExpression.Arguments[0]);

                    return dateTimeOffset.ToString((string)GetValueOfExpression(methodCallExpression.Arguments[1]));
                case nameof(ToString):
                    return VisitExpression(methodCallExpression.Object);
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
                    arguments[i] = GetValueOfExpression(newExpression.Arguments[i]);
                }

                if (newExpression.Type == typeof(DateTime) || newExpression.Type == typeof(DateTimeOffset))
                {
                    return ReflectionExtensions.ConvertToString(newExpression.Constructor.Invoke(arguments));
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
            var parameter = VisitParameterExpression(lambdaExpression.Parameters[0]);
            var filter = VisitExpression(lambdaExpression.Body);

            if (parameter == null)
            {
                parameter = lambdaExpression.Parameters[0].Name;
                return $"{parameter}:{parameter}/{filter}";
            }

            return $"{parameter}:{filter}";
        }

        protected virtual string VisitParameterExpression(ParameterExpression parameterExpression)
        {
            if (parameterExpression.Type == typeof(int) || parameterExpression.Type == typeof(string))
            {
                return parameterExpression.Name;
            }

            return default;
        }

        protected object GetValueOfExpression(Expression expression) => expression switch
        {
            MemberExpression memberExpression => GetValueOfMemberExpression(memberExpression),
            ConstantExpression constantExpression => GetValueOfConstantExpression(constantExpression),
            _ => default,
        };

        protected object GetValueOfConstantExpression(ConstantExpression constantExpression) =>
            constantExpression.Value;

        protected object GetValueOfMemberExpression(MemberExpression expression) => expression.Expression switch
        {
            ConstantExpression constantExpression => expression.Member.GetValue(constantExpression.Value),
            MemberExpression memberExpression => expression.Member.GetValue(GetValueOfMemberExpression(memberExpression)),
            _ => expression.Member.GetValue(),
        };

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

        public string ToString(Expression expression) => VisitExpression(expression);
    }
}
