using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Extensions;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Query
{
    internal class ODataQuery : IODataQuery
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly StringBuilder _stringBuilder;

        private static readonly char[] SeparatorUri = new char[2] { QuerySeparators.Begin, QuerySeparators.Main };
        private static readonly char[] SeparatorOperator = new char[1] { QuerySeparators.EqualSign };

        public ODataQuery(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IDictionary<string, string> ToDictionary()
        {
            var odataOperators = _stringBuilder
                .ToString()
                .Split(SeparatorUri, StringSplitOptions.RemoveEmptyEntries);

            var dictionary = new Dictionary<string, string>(odataOperators.Length);

            for (var step = 0; step < odataOperators.Length; step++)
            {
                var odataOperator = odataOperators[step]
                    .Split(SeparatorOperator, 2, StringSplitOptions.RemoveEmptyEntries);

                if (odataOperator.Length > 1)
                {
                    dictionary.Add(odataOperator[0], odataOperator[1]);
                }
            }

            return dictionary;
        }

        public Uri ToUri(UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            _stringBuilder.LastRemove(QuerySeparators.Begin);
            _stringBuilder.LastRemove(QuerySeparators.Main);

            return new Uri(_stringBuilder.ToString(), uriKind);
        }
    }
}
