using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.ExpressionVisitors
{
    internal class ODataExpressionVisitor : ExpressionVisitor
    {
        protected StringBuilder _queryBuilder;

        public ODataExpressionVisitor()
            : base() =>
            _queryBuilder = new StringBuilder();

        public string Query => _queryBuilder.ToString();

        protected override Expression VisitMember(MemberExpression node)
        {
            _queryBuilder.Append($"{node.Member.Name}");

            return base.VisitMember(node);
        }
    }
}
