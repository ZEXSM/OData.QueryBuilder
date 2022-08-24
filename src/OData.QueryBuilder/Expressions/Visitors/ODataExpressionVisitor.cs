using OData.QueryBuilder.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Expressions.Visitors
{
    internal class ODataExpressionVisitor
    {
        public ODataExpressionVisitor()
        {
        }

        protected string VisitExpression(LambdaExpression topExpression, Expression expression) => expression switch
        {
            BinaryExpression binaryExpression => VisitBinaryExpression(topExpression, binaryExpression),
            MemberExpression memberExpression => VisitMemberExpression(topExpression, memberExpression),
            ConstantExpression constantExpression => VisitConstantExpression(topExpression, constantExpression),
            MethodCallExpression methodCallExpression => VisitMethodCallExpression(topExpression, methodCallExpression),
            NewExpression newExpression => VisitNewExpression(topExpression, newExpression),
            UnaryExpression unaryExpression => VisitUnaryExpression(topExpression, unaryExpression),
            LambdaExpression lambdaExpression => VisitLambdaExpression(topExpression, lambdaExpression),
            ParameterExpression parameterExpression => VisitParameterExpression(topExpression, parameterExpression),
            _ => default,
        };

        [ExcludeFromCodeCoverage]
        protected virtual string VisitBinaryExpression(LambdaExpression topExpression, BinaryExpression binaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitConstantExpression(LambdaExpression topExpression, ConstantExpression constantExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitMethodCallExpression(LambdaExpression topExpression, MethodCallExpression methodCallExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitNewExpression(LambdaExpression topExpression, NewExpression newExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitUnaryExpression(LambdaExpression topExpression, UnaryExpression unaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitLambdaExpression(LambdaExpression topExpression, LambdaExpression lambdaExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitParameterExpression(LambdaExpression topExpression, ParameterExpression parameterExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitMemberExpression(LambdaExpression topExpression, MemberExpression memberExpression)
        {
            var memberName = VisitExpression(topExpression, memberExpression.Expression);

            if (string.IsNullOrEmpty(memberName))
            {
                return memberExpression.Member.Name;
            }

            return memberExpression.Member.DeclaringType.IsNullableType() ?
                memberName
                :
                $"{memberName}/{memberExpression.Member.Name}";
        }

        public virtual string ToQuery(LambdaExpression expression) =>
            VisitExpression(expression, expression.Body);
    }
}
