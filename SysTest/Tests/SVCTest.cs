using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    internal class SVCTest : Test
    {
        private string SvcName;
        public SVCTest(string name, string target, string svc)
        {
            this.Target = target;
            this.Type = TestType.TCP;
            this.Name = name;
            SvcName = svc;
        }

        public SVCTest(TestStructure ts)
        {
            this.Target = ts.Target;
            this.Type = ts.Type;
            this.Name = ts.Name;
            this.SvcName = ts.SvcName;
        }

        public override TestResult Run()
        {
            var sc = new ServiceController(SvcName, Target);

            try
            {
                var stat = sc.Status;
                if(stat == ServiceControllerStatus.Running)
                {
                    return new TestResult()
                    {
                        name = this.Name,
                        Success = true,
                        description = $"Service {this.SvcName} running on {this.Target}"
                    };
                }
                else
                {
                    return new TestResult()
                    {
                        name = this.Name,
                        Success = false,
                        description = $"Service {this.SvcName} not running on {this.Target}"
                    };
                }
            }
            catch (Exception e)
            {
                return new TestResult()
                {
                    name = this.Name,
                    Success = false,
                    description = $"Error: {e.Message}"
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
                SvcName = this.SvcName,
            };
        }
    }
}
