using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;

namespace OData.QueryBuilder.Test
{
    public class CommonFixture
    {
        public CommonFixture() =>
            ODataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata/");

        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder { get; private set; }
    }
}
