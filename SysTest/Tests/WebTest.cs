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
            this.target = target;
            this._type = TestType.Web;
        }

        public WebTest(TestStructure ts)
        {
            this.Name = ts.Name;
            this._type = ts.Type;
            this.ResponseCode = 200;
            this.target = ts.Target;
        }

        public override TestResult Run()
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, this.target);
            req.Headers.Add("User-Agent", "SysTest");
            var response = client.Send(req);

            if (response?.StatusCode == (HttpStatusCode)this.ResponseCode)
            {
                return new TestResult()
                {
                    Success = true,
                    description = $"Received {response?.StatusCode} from {target}",
                    name = Name
                };
            }
            else if (response != null)
            {
                return new TestResult()
                {
                    Success = false,
                    description = $"Received {response?.StatusCode} from {target}",
                    name = Name
                };
            }
            else
            {
                return new TestResult()
                {
                    Success = false,
                    description = $"Received no response from {target}",
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
                ResponseCode = this.ResponseCode,
            };
        }
    }
}
