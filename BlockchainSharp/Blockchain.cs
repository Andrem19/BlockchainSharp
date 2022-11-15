using BlockchainSharp.Services;
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
        public AccountModel _accountModel { get; private set; }
        public Blockchain()
        {
            _blocks = new List<Block>() { Block.Genesis() };
            _accountModel = new AccountModel();
        }
        public void AddBlock(Block block)
        {
            ExecuteTransactions(block._transactions);
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

        public List<Transaction> GetCoveredTransactionSet(List<Transaction> transactions)
        {
            List<Transaction> coveredTransactions = new List<Transaction>();

            foreach (Transaction transaction in transactions)
            {
                if (TransactionCovered(transaction))
                    coveredTransactions.Add(transaction);
                else
                    Console.WriteLine("transaction is not covered by sender");
            }
            return coveredTransactions;
        }
        public bool TransactionCovered(Transaction transaction)
        {
            if (transaction._type == "EXCHANGE")
                return true;
            decimal senderBalance = _accountModel.GetBalance(transaction._senderPublicKey);
            return senderBalance >= transaction._amount;
        }

        public void ExecuteTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                ExecuteTransaction(transaction);
            }
        }
        public void ExecuteTransaction(Transaction transaction)
        {
            string sender = transaction._senderPublicKey;
            string receiver = transaction._receiverPublicKey;
            decimal amount = transaction._amount;
            _accountModel.UpdateBalance(sender, -amount);
            _accountModel.UpdateBalance(receiver, amount);
        }
    }
}
