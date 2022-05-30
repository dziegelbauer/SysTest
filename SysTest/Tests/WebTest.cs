using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    internal class WebTest : Test
    {
        private uint ResponseCode = 200;

        public WebTest(string name, string target, uint code)
        {
            this.Name = name;
            this.ResponseCode = code;
            this.Target = target;
            this.Type = TestType.Web;
        }

        public WebTest(TestStructure ts)
        {
            this.Name = ts.Name;
            this.Type = ts.Type;
            this.ResponseCode = 200;
            this.Target = ts.Target;
        }

        public override TestResult Run()
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, this.Target);
            req.Headers.Add("User-Agent", "SysTest");
            var response = client.Send(req);

            if (response?.StatusCode == (HttpStatusCode)this.ResponseCode)
            {
                return new TestResult()
                {
                    Success = true,
                    description = $"Received {response?.StatusCode} from {Target}",
                    name = Name
                };
            }
            else if (response != null)
            {
                return new TestResult()
                {
                    Success = false,
                    description = $"Received {response?.StatusCode} from {Target}",
                    name = Name
                };
            }
            else
            {
                return new TestResult()
                {
                    Success = false,
                    description = $"Received no response from {Target}",
                    name = Name
                };
            }
        }

        public override TestStructure Serialize()
        {
            return new TestStructure()
            {
                Type = this.Type,
                Name = this.Name,
                Target = this.Target,
                ResponseCode = this.ResponseCode,
            };
        }
    }
}
