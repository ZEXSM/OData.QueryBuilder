using System;

namespace OData.QueryBuilder
{
    public static class ODataProperty
    {
        /// <summary>
        /// Dynamically resolved a property from the OData entity.
        /// </summary>
        /// <param name="propertyPath">The path to the property or field using dot separation for each path component.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        public static TProperty FromPath<TProperty>(string propertyPath)
        {
            throw new NotSupportedException("This method is only valid in OData query expressions.");
        }
    }
}