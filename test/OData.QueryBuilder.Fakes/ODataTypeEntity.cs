using System;
using System.Collections.Generic;

namespace OData.QueryBuilder.Fakes
{
    public class ODataTypeEntity
    {
        public Guid Id { get; set; }

        public int IdType { get; set; }

        public int? IdRule { get; set; }

        public string TypeCode { get; set; }

        public decimal Sum { get; set; }

        public bool IsActive { get; set; }

        public bool? IsOpen { get; set; }

        public DateTime Open { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset OpenDate { get; set; }

        public ODataKindEntity ODataKind { get; set; }

        public ODataKindEntity ODataKindNew { get; set; }

        public string[] Tags { get; set; }

        public List<string> Labels { get; set; }
    }
}

