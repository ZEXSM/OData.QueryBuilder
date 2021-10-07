using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Options
{
    public interface IODataOptionCollection<TODataOption, TEntity> : IODataOption<TODataOption, TEntity>
    {
        TODataOption Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false);

        http://docs.oasis-open.org/odata/odata-data-aggregation-ext/v4.0/cs01/odata-data-aggregation-ext-v4.0-cs01.html#_Toc378326289
        TODataOption Apply(Expression<Func<TEntity, bool>> apply);

        TODataOption OrderBy(Expression<Func<TEntity, object>> orderBy);

        TODataOption OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy);

        TODataOption OrderByDescending(Expression<Func<TEntity, object>> orderByDescending);

        TODataOption Top(int number);

        TODataOption Skip(int number);

        TODataOption Count(bool value = true);
    }
}
