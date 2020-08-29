using System.Collections.Generic;

namespace OData.QueryBuilder.Conventions.Functions
{
    public interface IReplaceFunction
    {
        string ReplaceCharacters(string value, IDictionary<string, string> keyValuePairs);
    }
}
