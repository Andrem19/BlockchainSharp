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
        public string _senderPublicKey { get; set; }
        public string _receiverPublicKey { get; set; }
        public decimal _amount { get; set; }
        public string _type { get; set; }
        public Guid _id { get; set; }
        public long _timestamp { get; set; }
        public string _signature { get; set; }

        public Transaction(string senderPublicKey, string receiverPublicKey, 
            decimal amount, string type)
        {
            _senderPublicKey= senderPublicKey;
            _receiverPublicKey= receiverPublicKey;
            _amount= amount;
            _type= type;
            _id = Guid.NewGuid();
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _signature = "";
        }
        public Transaction(string senderPublicKey, Guid id, long date, string receiverPublicKey,
            decimal amount, string type, string signature)
        {
            _senderPublicKey = senderPublicKey;
            _receiverPublicKey = receiverPublicKey;
            _amount = amount;
            _type = type;
            _id = id;
            _timestamp = date;
            _signature = signature;
        }
        public Transaction()
        {

        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void Sign(string signature)
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
