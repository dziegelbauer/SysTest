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
        Dictionary<Guid, Test> tests = new Dictionary<Guid, Test>();

        public bool Serialize(String file_path) {
            StringBuilder test_data = new StringBuilder();

            foreach (Test test in tests.Values) {
                test_data.Append(JsonSerializer.Serialize(test));
            }

            var tp_file = File.CreateText(file_path);

            tp_file.Write(test_data.ToString());

            tp_file.Close();

            return true; 
        }

        public bool Deserialize() { return true; }

        public Dictionary<Guid, Test> Tests()
        {
            return tests;
        }

        public void Clear()
        {
            tests.Clear();
        }
        
        public Guid AddTest(Test new_test)
        {
            var id = Guid.NewGuid();
            tests.Add(id, new_test);
            return id;
        }
    }
}
