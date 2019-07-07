using OData.QueryBuilder.Functions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryNestedParameter<TEntity>
    {
        IODataQueryNestedParameter<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> entityFilter);

        IODataQueryNestedParameter<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryNestedParameter<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataQueryNestedParameter<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataQueryNestedParameter<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);
    }
}
