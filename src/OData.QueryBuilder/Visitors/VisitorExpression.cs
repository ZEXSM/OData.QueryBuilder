using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Functions;
using OData.QueryBuilder.Operators;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Visitors
{
    internal class VisitorExpression
    {
        private readonly Expression _expression;

        public VisitorExpression(Expression expression) => _expression = expression;

        protected string VisitExpression(Expression expression) => expression switch
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

        protected string VisitBinaryExpression(BinaryExpression binaryExpression)
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

        protected string VisitMemberExpression(MemberExpression memberExpression) =>
            IsResourceOfMemberExpression(memberExpression) ?
                CreateResourcePath(memberExpression) : ReflectionExtensions.ConvertToString(GetValueOfMemberExpression(memberExpression));

        protected string VisitConstantExpression(ConstantExpression constantExpression) =>
            constantExpression.Value == default ?
                "null" : ReflectionExtensions.ConvertToString(constantExpression.Value);

        protected string VisitMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataQueryOperator.In):
                    var resourceIn = VisitExpression(methodCallExpression.Arguments[0]);
                    var filterIn = VisitExpression(methodCallExpression.Arguments[1]) ??
                        throw new ArgumentException("Enumeration is empty or null");

                    return $"{resourceIn} {ODataQueryOperators.In} ({filterIn})";
                case nameof(IODataQueryOperator.All):
                    var resourceAll = VisitExpression(methodCallExpression.Arguments[0]);
                    var filterAll = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{resourceAll}/{ODataQueryOperators.All}({filterAll})";
                case nameof(IODataQueryOperator.Any):
                    var resourceAny = VisitExpression(methodCallExpression.Arguments[0]);
                    var filterAny = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{resourceAny}/{ODataQueryOperators.Any}({filterAny})";
                case nameof(IODataQueryFunction.Date):
                    var filterDate = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{ODataQueryFunctions.Date}({filterDate})";
                case nameof(IODataQueryFunction.SubstringOf):
                    var substring = GetValueOfExpression(methodCallExpression.Arguments[0]);
                    var columName = VisitExpression(methodCallExpression.Arguments[1]);

                    return $"{ODataQueryFunctions.SubstringOf}('{substring}',{columName})";
                case nameof(IODataQueryStringFunction.ToUpper):
                    var filterTu = VisitExpression(methodCallExpression.Arguments[0]);

                    return $"{ODataQueryFunctions.ToUpper}({filterTu})";
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

        protected string VisitNewExpression(NewExpression newExpression)
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

            return string.Join(ODataQuerySeparators.CommaString, names);
        }

        protected string VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            var odataOperator = unaryExpression.NodeType.ToODataQueryOperator();
            var whitespace = odataOperator != default ? " " : default;

            return $"{odataOperator}{whitespace}{VisitExpression(unaryExpression.Operand)}";
        }

        protected string VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            var tag = lambdaExpression.Parameters[0]?.Name;
            var filter = VisitExpression(lambdaExpression.Body);

            return $"{tag}:{tag}/{filter}";
        }

        private object GetValueOfExpression(Expression expression) => expression switch
        {
            MemberExpression memberExpression => GetValueOfMemberExpression(memberExpression),
            ConstantExpression constantExpression => GetValueOfConstantExpression(constantExpression),
            _ => default,
        };

        private object GetValueOfConstantExpression(ConstantExpression constantExpression) =>
            constantExpression.Value;

        private object GetValueOfMemberExpression(MemberExpression expression) => expression.Expression switch
        {
            ConstantExpression constantExpression => expression.Member.GetValue(constantExpression.Value),
            MemberExpression memberExpression => expression.Member.GetValue(GetValueOfMemberExpression(memberExpression)),
            _ => expression.Member.GetValue(),
        };

        private bool IsResourceOfMemberExpression(MemberExpression memberExpression) => memberExpression.Expression switch
        {
            ParameterExpression pe => pe.Type.GetProperty(memberExpression.Member.Name, memberExpression.Type) != default,
            MemberExpression me => IsResourceOfMemberExpression(me),
            _ => false,
        };

        private string CreateResourcePath(MemberExpression memberExpression)
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
