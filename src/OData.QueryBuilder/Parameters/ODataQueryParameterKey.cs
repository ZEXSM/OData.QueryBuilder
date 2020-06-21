using OData.QueryBuilder.Builders.Nested;
using OData.QueryBuilder.Constants;
using System;
using System.Linq.Expressions;
using System.Text;

namespace OData.QueryBuilder.Parameters
{
    public class ODataQueryParameterKey<TEntity> : ODataQuery<TEntity>, IODataQueryParameterKey<TEntity>
    {
        public ODataQueryParameterKey(StringBuilder queryBuilder)
            : base(queryBuilder)
        {
        }

        public IODataQueryParameterKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand)
        {
            var visitor = new Visitor(entityExpand.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> actionEntityExpandNested)
        {
            var builder = new ODataQueryNestedBuilder<TEntity>();

            actionEntityExpandNested(builder);

            _stringBuilder.Append($"{ODataQueryParameters.Expand}{ODataQuerySeparators.EqualSignString}{builder.Query}{ODataQuerySeparators.MainString}");

            return this;
        }

        public IODataQueryParameterKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect)
        {
            var visitor = new Visitor(entitySelect.Body);
            var query = visitor.ToString();

            _stringBuilder.Append($"{ODataQueryParameters.Select}{ODataQuerySeparators.EqualSignString}{query}{ODataQuerySeparators.MainString}");

            return this;
        }
    }
}
