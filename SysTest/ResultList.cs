using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    internal class ResultList : ObservableCollection<TestResult>
    {
        public ResultList() : base()
        {
            this.Add(new TestResult { 
                description = "This test worked", 
                name = "First test", 
                Success = true 
            });

            this.Add(new TestResult
            {
                description = "This test failed",
                name = "Second test",
                Success = false
            });
        }
    }
}
