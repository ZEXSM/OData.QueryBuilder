using System;

namespace OData.QueryBuilder.Test
{
    public class CommonFixture
    {
        public CommonFixture()
        {
        }

        public string BaseUrl => "http://mock/odata";

        public Uri BaseUri => new Uri("http://mock/odata/");
    }
}
