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
                test_data.Append(JsonSerializer.Serialize<TestStructure>(test.Serialize()));
            }

            var tp_file = File.CreateText(file_path);

            tp_file.Write(test_data.ToString());

            tp_file.Close();

            return true; 
        }

        public bool Deserialize(String file_path) {
            if (File.Exists(file_path))
            {
                var tp_file = File.OpenText(file_path);
                var tp_string = tp_file.ReadToEnd();
                tp_file.Close();
                var tp_data = tp_string.Split('{', StringSplitOptions.RemoveEmptyEntries);

                tests.Clear();

                foreach (var s in tp_data)
                {
                    var tp_test = JsonSerializer.Deserialize<TestStructure>("{" + s);
                    
                    var id = Guid.NewGuid();
                    
                    switch(tp_test?.Type)
                    {
                        case TestType.Icmp:
                            tests.Add(id, new ICMPTest(tp_test));
                            break;
                        case TestType.Dns:
                            tests.Add(id, new DNSTest(tp_test));
                            break;
                        case TestType.Web:
                            tests.Add(id, new WebTest(tp_test));
                            break;
                        case TestType.TCP:
                            tests.Add(id, new TCPTest(tp_test));
                            break;
                        case TestType.SVC:
                            tests.Add(id, new SVCTest(tp_test));
                            break;
                        default:
                            break;
                    }
                }

                return true;
            }

            return false; 
        }

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
