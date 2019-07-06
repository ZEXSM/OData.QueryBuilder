using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder
{
    public interface IODataQueryBuilderList<TEntity>
    {
        IODataQueryBuilderList<TEntity> Filter(Expression<Func<IODataFunction, TEntity, bool>> entityFilter);

        IODataQueryBuilderList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataQueryBuilderList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataQueryBuilderList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataQueryBuilderList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IODataQueryBuilderList<TEntity> Top(int number);

        IODataQueryBuilderList<TEntity> Skip(int number);

        IODataQueryBuilderList<TEntity> Count();

        Uri ToUri();
    }
}