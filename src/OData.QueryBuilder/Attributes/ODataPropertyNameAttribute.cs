using System;

namespace OData.QueryBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ODataPropertyNameAttribute : Attribute
    {
        public string Name;

        public ODataPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
