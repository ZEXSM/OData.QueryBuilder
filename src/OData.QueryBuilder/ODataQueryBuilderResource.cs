using System.Text;

namespace OData.QueryBuilder
{
    public class ODataQueryBuilderResource<TEntity> : IODataQueryBuilderResource<TEntity>
    {
        private readonly StringBuilder _queryBuilder;

        public ODataQueryBuilderResource(string resourceUrl) =>
            _queryBuilder = new StringBuilder(resourceUrl);

        public IODataQueryBuilderKey<TEntity> ByKey(int key)
        {
            _queryBuilder.Append($"({key})?");

            return new ODataQueryBuilderKey<TEntity>(_queryBuilder);
        }

        public IODataQueryBuilderList<TEntity> ByList()
        {
            _queryBuilder.Append("?");

            return new ODataQueryBuilderList<TEntity>(_queryBuilder);
        }
    }
}
