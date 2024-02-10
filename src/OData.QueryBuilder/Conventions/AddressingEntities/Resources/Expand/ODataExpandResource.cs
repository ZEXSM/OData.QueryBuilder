using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Query.Expand;
using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Expressions.Visitors;
using OData.QueryBuilder.Options;
using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources.Expand
{
    internal class ODataExpandResource<TEntity> : IODataExpandResource<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly QBuilder _queryBuilder;
        private AbstractODataQueryExpand _odataQueryExpand;

        public QBuilder QueryBuilder
        {
            get
            {
                if (_odataQueryExpand?.QueryBuilder != null && !_odataQueryExpand.QueryBuilder.IsEmpty())
                {
                    return _queryBuilder.Append($"{QuerySeparators.LeftBracket}{_odataQueryExpand.QueryBuilder}{QuerySeparators.RigthBracket}");
                }

                return _queryBuilder;
            }
        }

        public ODataExpandResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _queryBuilder = new QBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataQueryExpand<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedExpand)
        {
            var query = new ODataResourceExpressionVisitor().ToString(nestedExpand);

            if (_odataQueryExpand?.QueryBuilder != null && !_odataQueryExpand.QueryBuilder.IsEmpty())
            {
                _queryBuilder.Append($"{QuerySeparators.LeftBracket}{_odataQueryExpand.QueryBuilder}{QuerySeparators.RigthBracket}{QuerySeparators.Comma}{query}");
            }
            else
            {
                if (query != null)
                {
                    if (!_queryBuilder.IsEmpty())
                    {
                        _queryBuilder.Append(QuerySeparators.Comma);
                    }

                    _queryBuilder.Append(query);
                }
            }

            _odataQueryExpand = new ODataQueryExpand<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataQueryExpand as ODataQueryExpand<TNestedEntity>;
        }
    }
}
