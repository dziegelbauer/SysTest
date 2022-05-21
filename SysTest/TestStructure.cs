using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    public class TestStructure
    {
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public DNSRecordType RecordType { get; set; }
        public ushort Port { get; set; }
        public TestStructure()
        {
            Name = "";
            Target = "";
        }

    }
}
