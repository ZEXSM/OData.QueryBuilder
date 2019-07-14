using OData.QueryBuilder.Extensions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters.Nested
{
    public class ODataQueryNestedParameter<TEntity> : ODataQueryNestedParameterBase, IODataQueryNestedParameter<TEntity>
    {
        public ODataQueryNestedParameter()
            : base()
        {
        }

        public IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand)
        {
            throw new NotImplementedException();
        }

        public IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect)
        {
            var selectNames = entityNestedSelect.Body.ToODataQuery(string.Empty);

            _queryBuilder.Append($"$select={selectNames};");

            return this;
        }
    }
}
