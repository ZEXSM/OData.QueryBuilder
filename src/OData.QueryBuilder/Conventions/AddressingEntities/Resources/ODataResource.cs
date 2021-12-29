﻿using OData.QueryBuilder.Options;
using System;
using System.Text;

namespace OData.QueryBuilder.Conventions.AddressingEntities.Resources
{
    internal class ODataResource : IODataResource
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;

        public ODataResource(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
            _stringBuilder = stringBuilder;
        }

        public IAddressingEntries<TEntity> For<TEntity>(string resource)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentException($"The specified resource name is null or empty", nameof(resource));
            }

            _stringBuilder.Append(resource);

            return new AddressingEntries<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
