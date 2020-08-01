using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options
{
    public interface IODataOptionList<TEntity> : IODataQuery
    {
        IODataOptionList<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter);

        IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter);

        IODataOptionList<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter);

        IODataOptionList<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IODataOptionList<TEntity> Expand(Action<IODataQueryExpandNestedBuilder<TEntity>> entityExpandNested);

        IODataOptionList<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IODataOptionList<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IODataOptionList<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IODataOptionList<TEntity> Top(int number);

        IODataOptionList<TEntity> Skip(int number);

        IODataOptionList<TEntity> Count(bool value = true);
    }
}