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
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource),"Resource name is null");
            }

            if (resource != string.Empty)
            {
                _stringBuilder.Append(resource);
            }

            return new AddressingEntries<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
