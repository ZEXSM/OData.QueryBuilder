using OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Options
{
    public interface IODataOption<TODataOption, TEntity>
    {
        TODataOption Expand(Expression<Func<TEntity, object>> expand);

        TODataOption Expand(Action<IODataExpandResource<TEntity>> expandNested);

        TODataOption Select(Expression<Func<TEntity, object>> select);
    }
}
