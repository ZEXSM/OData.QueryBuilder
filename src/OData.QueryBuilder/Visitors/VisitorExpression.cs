using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Visitors
{
    internal class VisitorExpression
    {
        private readonly Expression _expression;

        public VisitorExpression(Expression expression) => _expression = expression;

        protected virtual string VisitExpression(Expression expression) => expression switch
        {
            BinaryExpression binaryExpression => VisitBinaryExpression(binaryExpression),
            MemberExpression memberExpression => VisitMemberExpression(memberExpression),
            ConstantExpression constantExpression => VisitConstantExpression(constantExpression),
            MethodCallExpression methodCallExpression => VisitMethodCallExpression(methodCallExpression),
            NewExpression newExpression => VisitNewExpression(newExpression),
            UnaryExpression unaryExpression => VisitUnaryExpression(unaryExpression),
            LambdaExpression lambdaExpression => VisitLambdaExpression(lambdaExpression),
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
            constantExpression.Value == default ?
                "null" : ReflectionExtensions.ConvertToString(constantExpression.Value);

        protected virtual string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataOperator.In):
                    var in0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var in1 = VisitExpression(methodCallExpression.Arguments[1]) ??
                        throw new ArgumentException("Enumeration is empty or null");

                    return $"{in0} {ODataOperatorNames.In} ({in1})";
                case nameof(IODataOperator.All):
                    var all0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var all1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{all0}/{ODataOperatorNames.All}({all1})";
                case nameof(IODataOperator.Any):
                    var any0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var any1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{any0}/{ODataOperatorNames.Any}({any1})";
                case nameof(IODataFunction.Date):
                    var date0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{ODataFunctionNames.Date}({date0})";
                case nameof(IODataFunction.SubstringOf):
                    var substringOf0 = GetValueOfExpression(methodCallExpression.Arguments[0]);
                    var substringOf1 = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{ODataFunctionNames.SubstringOf}('{substringOf0}',{substringOf1})";
                case nameof(IODataFunction.Contains):
                    var contains0 = VisitExpression(methodCallExpression.Arguments[0]);
                    var contains1 = GetValueOfExpression(methodCallExpression.Arguments[1]);

                    return $"{ODataFunctionNames.Contains}({contains0},'{contains1}')";
                case nameof(IODataStringFunction.ToUpper):
                    var toUpper0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{ODataFunctionNames.ToUpper}({toUpper0})";
                case nameof(IODataStringFunction.ToLower):
                    var toLower0 = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{ODataFunctionNames.ToLower}({toLower0})";
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
            var tag = lambdaExpression.Parameters[0]?.Name;
            var filter = VisitExpression(lambdaExpression.Body);

            return $"{tag}:{tag}/{filter}";
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

        public new string ToString() => VisitExpression(_expression);
    }
}
