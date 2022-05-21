using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SysTest.Tests
{
    internal class TCPTest : Test
    {
        private ushort Port;
        public TCPTest(string name, string target, ushort port)
        {
            this.target = target;
            this._type = TestType.TCP;
            this.Name = name;
            Port = port;
        }

        public TCPTest(TestStructure ts)
        {
            this.target = ts.Target;
            this._type = ts.Type;
            this.Name = ts.Name;
            this.Port = ts.Port;
        }

        public override TestResult Run()
        {
            var ipaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });

            if(!IPAddress.TryParse(this.target, out ipaddr))
            {
                var resolution = Dns.GetHostEntry(this.target);
                if(resolution == null)
                {
                    return new TestResult()
                    {
                        name = this.Name,
                        Success = false,
                        description = $"Invalid target: {this.target}"
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
                            description = $"Successfully connected to {this.target} on port {this.Port}"
                        };
                    }
                    catch(SocketException e)
                    {
                        return new TestResult()
                        {
                            name = this.Name,
                            Success = false,
                            description = $"Socket exception connecting to {this.target}: {e.Message}"
                        };
                    }
                    catch(Exception err)
                    {
                        return new TestResult()
                        {
                            name = this.Name,
                            Success = false,
                            description = $"Unexpected error connecting to {this.target}: {err.Message}"
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
                    description = $"Invalid target: {this.target}"
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
                Port = this.Port,
            };
        }
    }
}
