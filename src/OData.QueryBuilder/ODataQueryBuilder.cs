using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder
{
    public class ODataQueryBuilder<TResource> : IODataQueryBuilder<TResource>
    {
        private readonly string _baseUrl;

        public ODataQueryBuilder(Uri baseUrl) =>
            _baseUrl = $"{baseUrl.OriginalString.TrimEnd('/')}/";

        public ODataQueryBuilder(string baseUrl) =>
            _baseUrl = $"{baseUrl.TrimEnd('/')}/";

        public IODataQueryBuilderResource<TEntity> ForResource<TEntity>(Expression<Func<TResource, object>> queryResource)
        {
            string[] queryResourceNames = default(string[]);

            switch (queryResource.Body)
            {
                case MemberExpression memberExpression:
                    queryResourceNames = new string[1];

                    queryResourceNames[0] = memberExpression.Member.Name;

                    break;
                default:
                    throw new NotSupportedException($"Выражение typeof {queryResource.Body.GetType().Name} не поддерживается.");
            }

            return new ODataQueryBuilderResource<TEntity>($"{_baseUrl}{queryResourceNames[0]}");
        }
    }
}