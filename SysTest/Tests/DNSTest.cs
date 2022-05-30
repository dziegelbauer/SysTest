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
        private string server;

        public DNSTest(string name, string target, DNSRecordType record_type, string server)
        {
            this.target = target;
            this._type = TestType.Dns;
            this.Name = name;
            this.rtype = record_type;
            this.server = server;
        }

        public DNSTest(TestStructure ts)
        {
            this.target = ts.Target;
            this._type = ts.Type;
            this.Name = ts.Name;
            this.rtype = ts.RecordType;
            this.server = ts.DNSServer;
        }

        public override TestResult Run()
        {
            var resolver = new DNS.Resolver();
            var QueryType = DNS.QType.ANY;
            if (rtype == DNSRecordType.PTR) QueryType = DNS.QType.PTR;
            if (rtype == DNSRecordType.A) QueryType = DNS.QType.A;
            if (rtype == DNSRecordType.AAAA) QueryType = DNS.QType.AAAA;
            if (rtype == DNSRecordType.NS) QueryType = DNS.QType.NS;

            resolver.DnsServer = server;
            resolver.UseCache = false;
            var result = resolver.Query(target, QueryType, DNS.QClass.ANY);

            if (result.Answers.Count > 0)
            {
                return new TestResult()
                {
                    Success = true,
                    description = $"Received {result.Answers.Count} results for {target}",
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

        /*
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
        */

        public override TestStructure Serialize()
        {
            return new TestStructure()
            {
                Type = this._type,
                Name = this.Name,
                Target = this.target,
                RecordType = this.rtype,
                DNSServer = this.server,
            };
        }
    }
}
