using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Test.Fakes
{
    public class ODataKindEntity
    {
        public int IdKind { get; set; }

        public ODataCodeEntity ODataCode { get; set; }

        public DateTimeOffset OpenDate { get; set; }
        public DateTime EndDate { get; set; }

        public ODataCodeEntity[] ODataCodes { get; set; }

        public IEnumerable<int> Sequence { get; set; }

        public int[] SequenceArray { get; set; }
    }
}
