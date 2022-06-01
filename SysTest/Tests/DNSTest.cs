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

        /// <summary>
		/// Create a new DNS test based on the provided parameters
		/// </summary>
		/// <param name="name">Name of the test (cosmetic)</param>
		/// <param name="target">DNS name to query</param>
        /// <param name="record_type">Type of record to query for (e.g. A, AAAA, etc)</param>
        /// <param name="server">DNS server to query against</param>
		/// <returns></returns>
        public DNSTest(string name, string target, DNSRecordType record_type, string server)
        {
            this.Target = target;
            this.Type = TestType.Dns;
            this.Name = name;
            this.rtype = record_type;
            this.server = server;
        }

        /// <summary>
		/// Create a new DNS test based on the provided TestStructure
		/// </summary>
		/// <param name="ts">TestStructure created by deserializing a JSON test plan</param>
		/// <returns></returns>
        public DNSTest(TestStructure ts)
        {
            this.Target = ts.Target;
            this.Type = ts.Type;
            this.Name = ts.Name;
            this.rtype = ts.RecordType;
            this.server = ts.DNSServer;
        }

        /// <summary>
		/// Execute the DNS test with the assigned parameters
		/// </summary>
		/// <returns>TestResult object indicating success or failure</returns>
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
            var result = resolver.Query(Target, QueryType, DNS.QClass.ANY);

            if(result.Error != "")
            {
                return new TestResult()
                {
                    Success = false,
                    description = result.Error,
                    name = Name
                };
            }

            if (result.Answers.Count > 0)
            {
                return new TestResult()
                {
                    Success = true,
                    description = $"Received {result.Answers.Count} results for {Target}",
                    name = Name
                };
            }
            else
            {
                var error_string = $"No results found for {Target}";

                return new TestResult()
                {
                    Success = false,
                    description = error_string,
                    name = Name
                };
            }
        }

        /// <summary>
		/// Create a TestStructure object to pass to the JSON serializer
		/// </summary>
		/// <returns>TestStructure object containing the test's data</returns>
        public override TestStructure Serialize()
        {
            return new TestStructure()
            {
                Type = this.Type,
                Name = this.Name,
                Target = this.Target,
                RecordType = this.rtype,
                DNSServer = this.server,
            };
        }
    }
}
