using OData.QueryBuilder.Constants;
using OData.QueryBuilder.Functions;
using OData.QueryBuilder.Operators;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class MethodCallExpressionExtensions
    {
        public static string ToODataQuery(this MethodCallExpression methodCallExpression, string queryString)
        {
            switch (methodCallExpression.Method.Name)
            {
                case nameof(IODataQueryOperator.In):
                    var resourceIn = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                    var filterIn = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                    return $"{resourceIn} {ODataQueryOperators.In} ({filterIn})";
                case nameof(IODataQueryOperator.All):
                    var resourceAll = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                    var filterAll = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                    return $"{resourceAll}/{ODataQueryOperators.All}({filterAll})";
                case nameof(IODataQueryOperator.Any):
                    var resourceAny = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                    var filterAny = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                    return $"{resourceAny}/{ODataQueryOperators.Any}({filterAny})";
                case nameof(IODataQueryFunction.Date):
                    var filterDate = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);

                    return $"{ODataQueryFunctions.Date}({filterDate})";
                case nameof(IODataQueryFunction.SubstringOf):
                    var resourceSof = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);
                    var filterSof = methodCallExpression.Arguments[1]?.ToODataQuery(queryString);

                    return $"{ODataQueryFunctions.SubstringOf}({resourceSof},{filterSof})";
                case nameof(string.ToUpper):
                    var filterTu = methodCallExpression.Arguments[0]?.ToODataQuery(queryString);

                    return $"{ODataQueryFunctions.ToUpper}({filterTu})";
                case nameof(ToString):
                    return methodCallExpression.Object.ToODataQuery();
                default:
                    return string.Empty;
            }
        }
    }
}
