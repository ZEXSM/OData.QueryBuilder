using OData.QueryBuilder.Conventions.Resources;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.Options
{
    public interface IAddressingEntriesKey<TEntity> : IODataQuery
    {
        IAddressingEntriesKey<TEntity> Expand(Expression<Func<TEntity, object>> entityExpand);

        IAddressingEntriesKey<TEntity> Expand(Action<IODataQueryExpandResource<TEntity>> entityExpandNested);

        IAddressingEntriesKey<TEntity> Select(Expression<Func<TEntity, object>> entitySelect);
    }
}
