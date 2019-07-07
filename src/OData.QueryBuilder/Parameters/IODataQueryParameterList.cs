using OData.QueryBuilder.Functions;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Parameters
{
    public interface IODataQueryParameterList<TEntity>
    {
        IODataQueryParameterList<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> entityFilter);

        IODataQueryParameterList<TEntity> Expand(Expression<Func<IODataQueryNestedParameter<TEntity>, TEntity, object>> entityExpand);

        IODataQueryParameterList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryParameterList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataQueryParameterList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataQueryParameterList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IODataQueryParameterList<TEntity> Top(int number);

        IODataQueryParameterList<TEntity> Skip(int number);

        IODataQueryParameterList<TEntity> Count();

        Uri ToUri();
    }
}