using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;

namespace OData.QueryBuilder.Test
{
    public class CommonFixture
    {
        public CommonFixture()
        {
            ODataQueryBuilder1 = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata/");
            ODataQueryBuilder2 = new ODataQueryBuilder<ODataInfoContainer>(new System.Uri("http://mock/odata/"));
        }

        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder1 { get; private set; }
        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder2 { get; private set; }
    }
}
