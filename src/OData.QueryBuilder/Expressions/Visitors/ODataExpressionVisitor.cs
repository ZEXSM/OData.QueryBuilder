using OData.QueryBuilder.Extensions;
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

        protected virtual string VisitBinaryExpression(BinaryExpression binaryExpression) => default;

        protected virtual string VisitConstantExpression(ConstantExpression constantExpression) => default;

        protected virtual string VisitMethodCallExpression(MethodCallExpression methodCallExpression) => default;

        protected virtual string VisitNewExpression(NewExpression newExpression) => default;

        protected virtual string VisitUnaryExpression(UnaryExpression unaryExpression) => default;

        protected virtual string VisitLambdaExpression(LambdaExpression lambdaExpression) => default;

        protected virtual string VisitParameterExpression(ParameterExpression parameterExpression) =>
            default;

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
