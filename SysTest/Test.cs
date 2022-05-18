using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    enum TestType
    {
        Web,
        Dns,
        Icmp
    }
    internal class Test
    {
        TestType _type;

        public TestType Type() { return _type; }
    }
}
