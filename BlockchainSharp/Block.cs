using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class Block
    {
        public int _blockCount { get; private set; }
        public List<Transaction> _transactions { get; private set; }
        public string _lastHash { get; private set; }
        public long _timastamp { get; private set; }
        public byte[] _forger { get; private set; }
        public byte[] _signature { get; private set; }

        public Block(List<Transaction> transactions, string lastHash, byte[] forger, int blockCount)
        {
            _blockCount= blockCount;
            _transactions = transactions;
            _lastHash= lastHash;
            _timastamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _forger= forger;
            _signature = new byte[0];
        }
        public static Block Genesis()
        {
            var genesisBlock = new Block(new List<Transaction>(), "genesisHash", Encoding.UTF8.GetBytes("genesis"), 0);
            genesisBlock._timastamp = 0;
            return genesisBlock;
        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string Payload()
        {
            string input = JsonConvert.SerializeObject(this);
            var json = JToken.Parse(input);
            json["_signature"] = "";
            return json.ToString();
        }
        public void Sign(byte[] signature)
        {
            _signature = signature;
        }
    }
}
