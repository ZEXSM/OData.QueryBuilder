using System;
using System.Collections.Generic;
using OData.QueryBuilder.Attributes;

namespace OData.QueryBuilder.Fakes
{
    public class ODataKindEntity
    {
        public int IdKind { get; set; }

        public ODataCodeEntity ODataCode { get; set; }

        public ODataCodeEntity ODataCodeNew { get; set; }

        public DateTimeOffset OpenDate { get; set; }
        public DateTime EndDate { get; set; }

        public ODataCodeEntity[] ODataCodes { get; set; }

        public IEnumerable<int> Sequence { get; set; }

        public int[] SequenceArray { get; set; }

        public long[] SequenceLongArray { get; set; }

        public ColorEnum Color { get; set; }

        [ODataPropertyName("customName")]
        public string CustomNamedProperty { get; set; }
    }
}
