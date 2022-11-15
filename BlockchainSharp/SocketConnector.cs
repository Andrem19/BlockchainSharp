using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class SocketConnector
    {
        public string _ip { get; set; }
        public string _port { get; set; }
        public SocketConnector(string ip, string port) 
        {
            _ip = ip;
            _port = port;
        }
        public bool Equals(SocketConnector connector)
        {
            if (connector._ip == _ip && connector._port == _port)
            {
                return true;
            }
            return false;
        }
    }
}
