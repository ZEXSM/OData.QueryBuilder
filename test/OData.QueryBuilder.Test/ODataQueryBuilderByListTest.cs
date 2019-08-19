using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryBuilderByListTest : IClassFixture<CommonFixture>
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilder;

        public static string IdCodeStatic => "testCode";

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

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter simple const int=> Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_Int_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3 or IdType eq 5");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter simple const string => Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_String_Success()
        {
            var constValue = "3";
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s =>
                     s.ODataKind.ODataCode.Code == constValue || s.ODataKind.ODataCode.Code == "5"
                     && s.ODataKind.ODataCode.Code == IdCodeStatic)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3' or ODataKind/ODataCode/Code eq '5' and ODataKind/ODataCode/Code eq 'testCode'");
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

        [Fact(DisplayName = "(ODataQueryBuilderList) Operator IN empty => Success")]
        public void ODataQueryBuilderList_Operator_In_Empty_Success()
        {
            var constStrIds = default(IEnumerable<string>);
            var constStrListIds = new string[] { }.ToList();
            var constIntIds = default(int[]);
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IdType in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter boolean values => Success")]
        public void ODataQueryBuilderList_Filter_Boolean_Values_Success()
        {
            var constValue = false;
            var newObject = new ODataTypeEntity { IsOpen = false };

            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive
                    && s.IsOpen == constValue
                    && s.IsOpen == true
                    && s.ODataKind.ODataCode.IdActive == newObject.IsOpen)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsActive and IsOpen eq false and IsOpen eq true and ODataKind/ODataCode/IdActive eq false");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter brackets => Success")]
        public void ODataQueryBuilderList_Filter_Brackets_Success()
        {
            var constStrIds = new[] { "123", "512" };
            var constValue = 3;

            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IdRule == constValue
                    && s.IsActive
                    && (((DateTimeOffset)s.EndDate).Date == default(DateTimeOffset?) || s.EndDate > DateTime.Today)
                    && (((DateTime)s.BeginDate).Date != default(DateTime?) || ((DateTime)s.BeginDate).Date <= DateTime.Today)
                    && constStrIds.Contains(s.ODataKind.ODataCode.Code))
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=IdRule eq 3 and IsActive and date(EndDate) eq null or EndDate gt {DateTime.Today.ToString("O")} and date(BeginDate) ne null or date(BeginDate) le {DateTime.Today.ToString("yyyy-MM-dd")} and ODataKind/ODataCode/Code in ('123','512')");
        }

        [Theory(DisplayName = "(ODataQueryBuilderList) Count value => Success")]
        [InlineData(true)]
        [InlineData(false)]
        public void ODataQueryBuilderList_Count_Value_Success(bool value)
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Count(value)
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$count={value.ToString().ToLower()}");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter not bool => Success")]
        public void ODataQueryBuilderList_Filter_Not__Bool_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive && !(bool)s.IsOpen)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsActive and not IsOpen");
        }
    }
}
