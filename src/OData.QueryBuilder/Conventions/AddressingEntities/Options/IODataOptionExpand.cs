using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Options
{
    public interface IODataOptionExpand<TODataOption, TEntity> : IODataOption<TODataOption, TEntity>
    {
        TODataOption Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false);

        TODataOption OrderBy(Expression<Func<TEntity, object>> orderBy);

        TODataOption OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy);

        TODataOption OrderByDescending(Expression<Func<TEntity, object>> orderByDescending);

        TODataOption Top(int number);
    }
}
