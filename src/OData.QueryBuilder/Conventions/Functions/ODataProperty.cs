using System;

namespace OData.QueryBuilder
{
    public static class ODataProperty
    {
        /// <summary>
        /// Dynamic property
        /// </summary>
        /// <param name="propertyPath">The path to the property or field.</param>
        /// <typeparam name="T">The type of the property.</typeparam>
        public static T FromPath<T>(string propertyPath)
        {
            throw new NotSupportedException("This method is only valid in OData query expressions.");
        }
    }
}