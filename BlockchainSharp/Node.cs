using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class Node
    {
        //public SocketCommunication _p2p { get; set; }
        public string _ip { get; set; }
        public string _port { get; set; }
        public Blockchain _blockchain { get; set; }
        public TransactionPool _transactionPool { get; set; }
        public Wallet _wallet { get; set; }

        public Node(string ip, string port)
        {
            _blockchain = new Blockchain();
            _transactionPool = new TransactionPool();
            _wallet = new Wallet();
            //_p2p = null;
            _ip = ip;
            _port = port;
        }

        public async Task StartP2P()
        {
            SocketCommunication.SocetConnectorInit(_ip, _port);
            await SocketCommunication.StartSocketCommunication(_port);
        }
    }
}
