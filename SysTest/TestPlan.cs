using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SysTest
{
    internal class TestPlan
    {
        List<Test> tests = new List<Test>();

        public bool Serialize(String file_path) {
            StringBuilder test_data = new StringBuilder();

            foreach (Test test in tests) {
                test_data.Append(JsonSerializer.Serialize(test));
            }

            var tp_file = File.CreateText(file_path);

            tp_file.Write(test_data.ToString());

            tp_file.Close();

            return true; 
        }

        public bool Deserialize() { return true; }

        public List<Test> Tests()
        {
            return tests;
        }
        
        public void AddTest(Test new_test)
        {
            tests.Add(new_test);
        }
    }
}
