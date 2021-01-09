using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace OData.QueryBuilder
{
    internal class ODataQuery<TEntity> : IODataQuery
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly StringBuilder _stringBuilder;

        public ODataQuery(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public Dictionary<string, string> ToDictionary()
        {
            var odataOperators = _stringBuilder.ToString()
                .Split(new char[2] { QuerySeparators.Begin, QuerySeparators.Main }, StringSplitOptions.RemoveEmptyEntries);

            var dictionary = new Dictionary<string, string>(odataOperators.Length - 1);

            for (var step = 1; step < odataOperators.Length; step++)
            {
                var odataOperator = odataOperators[step].Split(QuerySeparators.EqualSign);

                dictionary.Add(odataOperator[0], odataOperator[1]);
            }

            return dictionary;
        }

        public Uri ToUri() => new Uri(_stringBuilder.ToString().TrimEnd(QuerySeparators.Main));
    }
}
