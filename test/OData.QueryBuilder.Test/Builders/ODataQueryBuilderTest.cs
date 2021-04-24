using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using System;
using Xunit;

namespace OData.QueryBuilder.Test.Builders
{
    public class ODataQueryBuilderTest
    {
        [Theory(DisplayName = "new instance ODataQueryBuilder with baseUrl => Exception")]
        [InlineData("")]
        [InlineData(null)]
        public void ODataQueryBuilder_New_Instance_BaseUrl_Exception(string baseUrl)
        {
            var ex = Assert.Throws<ArgumentException>(() => new ODataQueryBuilder<ODataInfoContainer>(baseUrl));
            ex.Message.Should().Be($"{nameof(baseUrl)} is null");
        }

        [Fact(DisplayName = "new instance ODataQueryBuilder with baseUrl Uri => Exception")]
        public void ODataQueryBuilder_New_Instance_BaseUrl_Uri_Exception()
        {
            var uri = default(System.Uri);

            var ex = Assert.Throws<ArgumentException>(() => new ODataQueryBuilder<ODataInfoContainer>(uri));
            ex.Message.Should().Be($"baseUrl is null");
        }
    }
}
