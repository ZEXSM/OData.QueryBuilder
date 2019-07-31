using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;
using System;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryBuilderByListTest : IClassFixture<CommonFixture>
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilder;

        public ODataQueryBuilderByListTest(CommonFixture commonFixture) =>
            _odataQueryBuilder = commonFixture.ODataQueryBuilder;

        [Fact(DisplayName = "(ODataQueryBuilderList) Expand simple => Success")]
        public void ODataQueryBuilderList_Expand_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { s.ODataKind })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKind");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Expand nested => Success")]
        public void ODataQueryBuilderList_ExpandNested_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Expand(ff => ff.For<ODataCodeEntity>(s => s.ODataCode).Select(s => s.IdCode))
                        .Select(s => s.IdKind);
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind);
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind);
                })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKind($expand=ODataCode($select=IdCode);$select=IdKind),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Select simple => Success")]
        public void ODataQueryBuilderList_Select_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Select(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$select=IdType");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) OrderBy simple => Success")]
        public void ODataQueryBuilderList_OrderBy_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderBy(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$orderby=IdType asc");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) OrderByDescending simple => Success")]
        public void ODataQueryBuilderList_OrderByDescending_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderByDescending(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$orderby=IdType desc");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Count simple => Success")]
        public void ODataQueryBuilderList_Count_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Count()
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$count=true");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Skip and Top simple => Success")]
        public void ODataQueryBuilderList_Skip_Top_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Skip(1)
                .Top(1)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$skip=1&$top=1");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter simple => Success")]
        public void ODataQueryBuilderList_Filter_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3 or IdType eq 5");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter All/Any => Success")]
        public void ODataQueryBuilderList_Filter_All_Any_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.ODataKind.ODataCodes.Any(v => v.IdCode == 1)
                    && s.ODataKind.ODataCodes.All(v => v.IdActive))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCodes/any(v:v/IdCode eq 1) and ODataKind/ODataCodes/all(v:v/IdActive)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Expand,Filter,Select,OrderBy,OrderByDescending,Skip,Top,Count => Success")]
        public void ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count_Success()
        {
            var constValue = 2;
            var constCurrentDate = DateTime.Today.ToString("yyyy-MM-dd");

            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { s.ODataKind })
                .Filter(s =>
                    (s.IdType < constValue && s.ODataKind.ODataCode.IdCode >= 3)
                    || s.IdType == 5
                    && s.IdRule != default(int?)
                    && s.IdRule == null
                    )
                .Select(s => new { s.ODataKind, s.Sum })
                .OrderBy(s => new { s.IdType })
                .OrderByDescending(s => s.IdType)
                .Skip(1)
                .Top(1)
                .Count()
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$expand=ODataKind&$filter=IdType lt 2 and ODataKind/ODataCode/IdCode ge 3 or IdType eq 5 and IdRule ne null and IdRule eq null&$select=ODataKind,Sum&$orderby=IdType asc&$orderby=IdType desc&$skip=1&$top=1&$count=true");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Function Date => Success")]
        public void ODataQueryBuilderList_Function_Date_Success()
        {
            var constCurrentDateToday = new DateTime(2019, 2, 9);
            var constCurrentDateNow = new DateTime(2019, 2, 9, 1, 2, 4);
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { EndDate = constCurrentDateToday } };

            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s =>
                    s.ODataKind.OpenDate.Date == constCurrentDateNow
                    && s.ODataKind.OpenDate == constCurrentDateToday
                    && s.ODataKind.OpenDate == DateTime.Today
                    && s.Open.Date == DateTime.Today
                    && s.Open == DateTime.Today
                    && s.Open == constCurrentDateToday
                    && s.Open.Date == newObject.ODataKind.EndDate
                    && s.ODataKind.OpenDate.Date == new DateTime(2019, 7, 9)
                    && ((DateTime)s.BeginDate).Date == DateTime.Today)
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=date(ODataKind/OpenDate) eq 2019-02-09 and ODataKind/OpenDate eq 2019-02-09T00:00:00.0000000 and ODataKind/OpenDate eq {DateTime.Today.ToString("O")} and date(Open) eq {DateTime.Today.ToString("yyyy-MM-dd")} and Open eq {DateTime.Today.ToString("O")} and Open eq 2019-02-09T00:00:00.0000000 and date(Open) eq 2019-02-09 and date(ODataKind/OpenDate) eq 2019-07-09 and date(BeginDate) eq {DateTime.Today.ToString("yyyy-MM-dd")}");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Operator IN => Success")]
        public void ODataQueryBuilderList_Operator_In_Success()
        {
            var constStrIds = new[] { "123", "512" };
            var constStrListIds = new[] { "123", "512" }.ToList();
            var constIntIds = new[] { 123, 512 };
            var constIntListIds = new[] { 123, 512 }.ToList();
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
            var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => constStrIds.Contains(s.ODataKind.ODataCode.Code)
                    && constStrListIds.Contains(s.ODataKind.ODataCode.Code)
                    && constIntIds.Contains(s.IdType)
                    && constIntListIds.Contains(s.IdType)
                    && constIntIds.Contains((int)s.IdRule)
                    && constIntListIds.Contains((int)s.IdRule)
                    && newObject.ODataKind.Sequence.Contains(s.ODataKind.IdKind)
                    && newObjectSequenceArray.ODataKind.SequenceArray.Contains(s.ODataKind.ODataCode.IdCode))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512) and IdType in (123,512) and IdRule in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512) and ODataKind/ODataCode/IdCode in (123,512)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter boolean values => Success")]
        public void ODataQueryBuilderList_Filter_Boolean_Values_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive && s.IsOpen == true && s.ODataKind.ODataCode.IdActive == false)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsActive and IsOpen eq true and ODataKind/ODataCode/IdActive eq false");
        }
    }
}
