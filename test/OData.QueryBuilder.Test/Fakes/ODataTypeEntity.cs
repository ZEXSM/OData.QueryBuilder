using System;

namespace OData.QueryBuilder.Test.Fakes
{
    public class ODataTypeEntity
    {
        public int IdType { get; set; }

        public int? IdRule { get; set; }

        public string TypeCode { get; set; }

        public decimal Sum { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpen { get; set; }

        public DateTime Open { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTimeOffset OpenDate { get; set; }

        public ODataKindEntity ODataKind { get; set; }

        public ODataKindEntity ODataKindNew { get; set; }
    }
}

