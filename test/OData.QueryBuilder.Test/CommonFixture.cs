namespace OData.QueryBuilder.Test
{
    public class CommonFixture
    {
        public CommonFixture()
        {
        }

        public string BaseUrl => "http://mock/odata/";

        public System.Uri BaseUri => new System.Uri("http://mock/odata/");
    }
}
