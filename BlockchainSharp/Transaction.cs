using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class Transaction
    {
        public byte[] _senderPublicKey { get; private set; }
        public string _receiverPublicKey { get; private set; }
        public decimal _amount { get; private set; }
        public string _type { get; private set; }
        public Guid _id { get; private set; }
        public long _timestamp { get; private set; }
        public byte[] _signature { get; private set; }

        public Transaction(byte[] senderPublicKey, string receiverPublicKey, 
            decimal amount, string type)
        {
            _senderPublicKey= senderPublicKey;
            _receiverPublicKey= receiverPublicKey;
            _amount= amount;
            _type= type;
            _id = Guid.NewGuid();
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _signature = new byte[0];
        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void Sign(byte[] signature)
        {
            _signature = signature;
        }
        public bool Equals(Transaction transaction)
        {
            if (this._id == transaction._id)
                return true;
            else return false;
        }
        public string Payload()
        {
            string input = JsonConvert.SerializeObject(this);
            var json = JToken.Parse(input);
            json["_signature"] = "";
            return json.ToString();
        }
    }
}
