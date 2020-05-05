using OData.QueryBuilder.Extensions;
using System.Linq.Expressions;

namespace OData.QueryBuilder.ExpressionVisitors
{
    internal class FilterODataExpressionVisitor : ODataExpressionVisitor
    {
        public FilterODataExpressionVisitor()
            : base()
        {
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);

            _queryBuilder.Append($" {node.GetODataLogicalOperator()} ");

            Visit(node.Right);

            return default;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            switch (node.Value)
            {
                case bool b:
                    _queryBuilder.Append(b.ToString().ToLower());
                    break;
                case int i:
                    _queryBuilder.Append(i);
                    break;
                case string s:
                    _queryBuilder.Append($"'{s}'");
                    break;
                case object o:
                    _queryBuilder.Append($"'{o}'");
                    break;
                default:
                    _queryBuilder.Append("null");
                    break;
            }

            return base.VisitConstant(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
        }
    }
}
