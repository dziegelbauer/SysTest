using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    public enum TestType
    {
        Web,
        Dns,
        Icmp,
        TCP,
        SVC
    }
    public class Test
    {
        protected TestType Type;
        protected string Name = "";
        protected string Target = "";

        public new TestType GetType() { return Type; }

        public virtual TestResult Run()
        {
            Random random = new Random();
            bool success = false;

            if(random.Next(0,2) == 0)
            {
                success = false;
            }
            else
            {
                success = true;
            }

            var r = new TestResult
            {
                name = $"Test{random.Next(100)}",
                description = success ? "Test passed" : "Test failed",
                Success = success
            };

            return r;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual TestStructure Serialize()
        {
            return new TestStructure()
            {
                Type = this.Type,
                Name = this.Name,
                Target = this.Target,
            };
        }
    }
}
