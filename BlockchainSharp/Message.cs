using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    class Message
    {
        public string _sender { get; set; }
        public string _messageType { get; set; }
        public string _data { get; set; }

        public Message(string sender, string messageType, string data)
        {
            _sender = sender;
            _messageType = messageType;
            _data = data;
        }
        public static string CreateMessage(string sender, string messageType, string data)
        {
            var msg = new Message(sender, messageType, data);
            string msgStr = JsonConvert.SerializeObject(msg);
            return msgStr;
        }
    }
    
}
