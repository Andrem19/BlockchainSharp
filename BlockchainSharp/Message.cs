using BlockchainSharp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class Message
    {
        public SocketConnector _senderConnector { get; set; }
        public string _messageType { get; set; }
        public string _data { get; set; }
        public Message(SocketConnector senderConnector, string messageType, string data)
        {
            _senderConnector = senderConnector;
            _messageType = messageType;
            _data = data;
        }
    }
}
