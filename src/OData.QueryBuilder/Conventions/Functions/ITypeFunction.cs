namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_TypeFunctions
    /// </summary>
    public interface ITypeFunction
    {
        /// <summary>
        /// https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_cast
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="type">https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/entity-data-model-primitive-data-types#primitive-data-types-supported-in-the-entity-data-model</param>
        /// <returns></returns>
        string Cast(string columnName, string type);
    }
}
