using OData.QueryBuilder.Builders;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    public interface IODataOptionNested<TEntity>
    {
        IODataOptionNested<TEntity> Expand(Action<IODataQueryNestedBuilder<TEntity>> entityExpandNested);

        IODataOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand);

        IODataOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter);

        IODataOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect);

        IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy);

        IODataOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending);

        IODataOptionNested<TEntity> Top(int number);
    }
}
