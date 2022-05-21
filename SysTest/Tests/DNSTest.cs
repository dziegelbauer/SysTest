using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    public enum DNSRecordType
    {
        A,
        AAAA,
        PTR,
        NS,
        ANY
    }
    internal class DNSTest : Test
    {
        private DNSRecordType rtype = DNSRecordType.ANY;
        public DNSTest(string name, string target, DNSRecordType record_type)
        {
            this.target = target;
            this._type = TestType.Dns;
            this.Name = name;
            this.rtype = record_type;
        }

        public DNSTest(TestStructure ts)
        {
            this.target = ts.Target;
            this._type = ts.Type;
            this.Name = ts.Name;
            this.rtype = ts.RecordType;
        }

        public override TestResult Run()
        {
            var reply = Dns.GetHostAddresses(target);

            
            if (reply != null)
            {
                return new TestResult()
                {
                    Success = true,
                    description = $"Received {reply.Count()} results for {target}",
                    name = Name
                };
            }
            else
            {
                var error_string = $"No results found for {target}";
                      
                return new TestResult()
                {
                    Success = false,
                    description = error_string,
                    name = Name
                };
            }
        }

        public override TestStructure Serialize()
        {
            return new TestStructure()
            {
                Type = this._type,
                Name = this.Name,
                Target = this.target,
                RecordType = this.rtype
            };
        }
    }
}
