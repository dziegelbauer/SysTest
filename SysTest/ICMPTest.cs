using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    internal class ICMPTest : Test
    {
        public ICMPTest(string name, string target)
        {
            this.target = target;
            this._type = TestType.Icmp;
            this.Name = name;            
        }

        public ICMPTest(TestStructure ts)
        {
            this.target = ts.Target;
            this._type = ts.Type;
            this.Name = ts.Name;
        }

        public override TestResult Run()
        {
            var ping_sender = new Ping();
            var options = new PingOptions();

            var reply = ping_sender.Send(target);

            if(reply.Status == IPStatus.Success)
            {
                return new TestResult() { 
                    Success = true,
                    description = $"Received reply from {reply.Address.ToString()}",
                    name = Name
                };
            }
            else
            {
                var error_string = "";
                switch (reply.Status)
                {
                    case IPStatus.TimedOut:
                        error_string = $"Timeout waiting for {target}";
                        break;
                    case IPStatus.DestinationHostUnreachable:
                        error_string = $"Destination host unreachable sending to {target}";
                        break;
                    case IPStatus.DestinationUnreachable:
                        error_string = $"Destination unreachable sending to {target}";
                        break;
                    case IPStatus.DestinationNetworkUnreachable:
                        error_string = $"Destination network unreachable sending to {target}";
                        break;
                    default:
                        error_string = $"{reply.Status.ToString()} sending to {target}";
                        break;
                }
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
            };
        }
    }
}
