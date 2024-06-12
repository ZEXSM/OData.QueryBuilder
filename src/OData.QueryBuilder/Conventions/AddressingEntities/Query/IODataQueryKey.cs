using OData.QueryBuilder.Conventions.AddressingEntities.Options;
using System;
using System.Linq.Expressions;
using OData.QueryBuilder.Conventions.Functions;
using OData.QueryBuilder.Conventions.Operators;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    public interface IODataQueryKey<TEntity> : IODataOptionKey<IODataQueryKey<TEntity>, TEntity>, IODataQuery
    {
        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource);
        IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false);
        IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false);
        IODataQueryKey<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false);
    }
}
