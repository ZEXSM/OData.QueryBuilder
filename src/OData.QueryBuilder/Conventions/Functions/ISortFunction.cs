namespace OData.QueryBuilder.Conventions.Functions
{
    public interface ISortFunction
    {
        ISortFunction Ascending<T>(T column);

        ISortFunction Descending<T>(T column);
    }
}
