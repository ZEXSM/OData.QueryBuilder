using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Conventions.Resources;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options.Expand
{
    public interface IODataOptionExpand<TEntity>
    {
        IODataOptionExpand<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> entityExpandNested);

        IODataOptionExpand<TEntity> Expand(Expression<Func<TEntity, object>> entityNestedExpand);

        IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, bool>> entityNestedFilter, bool useParenthesis = false);

        IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter, bool useParenthesis = false);

        IODataOptionExpand<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter, bool useParenthesis = false);

        IODataOptionExpand<TEntity> Select(Expression<Func<TEntity, object>> entityNestedSelect);

        IODataOptionExpand<TEntity> OrderBy(Expression<Func<TEntity, object>> entityNestedOrderBy);

        IODataOptionExpand<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> entityOrderBy);

        IODataOptionExpand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityNestedOrderByDescending);

        IODataOptionExpand<TEntity> Top(int number);
    }
}
