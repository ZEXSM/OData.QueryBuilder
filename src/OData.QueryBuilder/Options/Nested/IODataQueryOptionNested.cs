using OData.QueryBuilder.Builders.Nested;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Options.Nested
{
    public interface IODataQueryOptionNested<TEntity>
    {
        IODataQueryOptionNested<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataQueryOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand);

        IODataQueryOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter);

        IODataQueryOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect);

        IODataQueryOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy);

        IODataQueryOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending);

        IODataQueryOptionNested<TEntity> Top(int number);
    }
}
