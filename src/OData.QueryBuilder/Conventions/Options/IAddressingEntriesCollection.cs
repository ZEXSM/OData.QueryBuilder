using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;
using OData.QueryBuilder.Conventions.Resources;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options
{
    public interface IAddressingEntriesCollection<TEntity> : IODataQuery
    {
        IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, bool>> entityFilter, bool useParenthesis = false);

        IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> entityFilter, bool useParenthesis = false);

        IAddressingEntriesCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> entityFilter, bool useParenthesis = false);

        IAddressingEntriesCollection<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IAddressingEntriesCollection<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> entityExpandNested);

        IAddressingEntriesCollection<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);

        IAddressingEntriesCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> entityOrderBy);

        IAddressingEntriesCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> entityOrderBy);

        IAddressingEntriesCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> entityOrderByDescending);

        IAddressingEntriesCollection<TEntity> Top(int number);

        IAddressingEntriesCollection<TEntity> Skip(int number);

        IAddressingEntriesCollection<TEntity> Count(bool value = true);
    }
}