using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class Blockchain
    {
        public List<Block> _blocks { get; private set; }
        public Blockchain()
        {
            _blocks = new List<Block>() { Block.Genesis() };
        }
        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }
        public string toJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public bool BlockCountValid(Block block)
        {
            return _blocks[_blocks.Count - 1]._blockCount == block._blockCount - 1;
        }
        public bool LastBlockHashValid(Block block)
        {
            byte[] lastBlockHash = BlockchainUtils.Hash(_blocks[_blocks.Count - 1].Payload());
            return lastBlockHash.ByteArrayToString() == block._lastHash;
        }
    }
}
