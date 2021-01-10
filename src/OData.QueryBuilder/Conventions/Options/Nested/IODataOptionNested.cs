using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Resources;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options.Nested
{
    public interface IODataOptionNested<TEntity>
    {
        IODataOptionNested<TEntity> Expand(Action<IODataQueryExpandNestedResource<TEntity>> entityExpandNested);

        IODataOptionNested<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand);

        IODataOptionNested<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter, bool useParenthesis = false);

        IODataOptionNested<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect);

        IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy);

        IODataOptionNested<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> entityOrderBy);

        IODataOptionNested<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending);

        IODataOptionNested<TEntity> Top(int number);
    }
}
