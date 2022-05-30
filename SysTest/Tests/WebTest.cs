using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    internal class WebTest : Test
    {
        private uint ResponseCode = 200;

        public WebTest(string name, string target, uint code)
        {

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
            throw new NotImplementedException();
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
