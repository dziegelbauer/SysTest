using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SysTest
{
    public class TCPTest : Test
    {
        private ushort Port;
        public TCPTest(string name, string target, ushort port)
        {
            this.Target = target;
            this.Type = TestType.TCP;
            this.Name = name;
            Port = port;
        }

        public TCPTest(TestStructure ts)
        {
            this.Target = ts.Target;
            this.Type = ts.Type;
            this.Name = ts.Name;
            this.Port = ts.Port;
        }

        public override TestResult Run()
        {
            var ipaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });

            if(!IPAddress.TryParse(this.Target, out ipaddr))
            {
                var resolution = Dns.GetHostEntry(this.Target);
                if(resolution == null)
                {
                    return new TestResult()
                    {
                        name = this.Name,
                        Success = false,
                        description = $"Invalid target: {this.Target}"
                    };
                }
                else
                {
                    var ep = new IPEndPoint(resolution.AddressList[0], this.Port);

                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        sock.Connect(ep);
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();

                        return new TestResult()
                        {
                            name = this.Name,
                            Success = true,
                            description = $"Successfully connected to {this.Target} on port {this.Port}"
                        };
                    }
                    catch(SocketException e)
                    {
                        return new TestResult()
                        {
                            name = this.Name,
                            Success = false,
                            description = $"Socket exception connecting to {this.Target}: {e.Message}"
                        };
                    }
                    catch(Exception err)
                    {
                        return new TestResult()
                        {
                            name = this.Name,
                            Success = false,
                            description = $"Unexpected error connecting to {this.Target}: {err.Message}"
                        };
                    }
                }
            }
            else
            {
                return new TestResult()
                {
                    name = this.Name,
                    Success = false,
                    description = $"Invalid target: {this.Target}"
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
                Port = this.Port,
            };
        }
    }
}
