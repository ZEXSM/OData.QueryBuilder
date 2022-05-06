namespace OData.QueryBuilder.Conventions.Functions
{
    /// <summary>
    /// OData functions
    /// </summary>
    public interface IODataFunction : IODataStringAndCollectionFunction, IODataDateTimeFunction, ICustomFunction, ITypeFunction
    {
    }
}
