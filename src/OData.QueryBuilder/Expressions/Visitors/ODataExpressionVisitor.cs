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

        protected string VisitExpression(Expression expression) => expression switch
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

        [ExcludeFromCodeCoverage]
        protected virtual string VisitBinaryExpression(BinaryExpression binaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitConstantExpression(ConstantExpression constantExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitMethodCallExpression(MethodCallExpression methodCallExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitNewExpression(NewExpression newExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitUnaryExpression(UnaryExpression unaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitLambdaExpression(LambdaExpression lambdaExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitParameterExpression(ParameterExpression parameterExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitMemberExpression(MemberExpression memberExpression)
        {
            var memberName = VisitExpression(memberExpression.Expression);

            if (string.IsNullOrEmpty(memberName))
            {
                return memberExpression.Member.Name;
            }

            return memberExpression.Member.DeclaringType.IsNullableType() ?
                memberName
                :
                $"{memberName}/{memberExpression.Member.Name}";
        }

        public virtual string ToQuery(Expression expression) =>
            VisitExpression(expression);
    }
}
