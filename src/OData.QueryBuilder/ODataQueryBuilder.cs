using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Resourses;
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

        public IODataQueryResource<TEntity> ForResource<TEntity>(Expression<Func<TResource, object>> queryResource)
        {
            var queryResourceNames = default(string);

            switch (queryResource.Body)
            {
                case MemberExpression memberExpression:
                    queryResourceNames = memberExpression.ToODataQuery();
                    break;

                default:
                    throw new NotSupportedException($"Выражение {queryResource.Body.GetType().Name} не поддерживается.");
            }

            return new ODataQueryResource<TEntity>($"{_baseUrl}{queryResourceNames}");
        }
    }
}