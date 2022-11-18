using BlockchainSharp;
using Microsoft.Extensions.Logging;

namespace SharpChain
{
	class Program
	{
		static async Task Main(string[] args)
		{
            //string sender = "sender";
            //string receiver = "receiver";
            //int amount = 100;
            //string type = "TRANSFER";

            //var wallet = new Wallet();
            //var fraudFallet = new Wallet();
            //var pool = new TransactionPool();

            //var transaction = wallet.CreateTransaction(receiver, amount, type);



            //string transactionPayload = transaction.Payload();

            //bool signatureValid = Wallet.SignatureValid(transactionPayload, transaction._signature, wallet.GetPublicKeyString());

            //if (signatureValid)
            //{
            //	Console.WriteLine("transaction signature is valid");
            //}



            //if (!pool.TransactionExist(transaction))
            //	pool.AddTransaction(transaction);

            //var blockchain = new Blockchain();

            //byte[] lastHash = BlockchainUtils.Hash(blockchain._blocks[blockchain._blocks.Count - 1].Payload());
            //int blockCount = blockchain._blocks[blockchain._blocks.Count - 1]._blockCount + 1;
            //var block = wallet.CreateBlock(pool.transactions, lastHash.ByteArrayToString(), blockCount);

            //if (!blockchain.LastBlockHashValid(block))
            //{
            //	Console.WriteLine("Last block hash is not valid");
            //}
            //         if (!blockchain.BlockCountValid(block))
            //         {
            //             Console.WriteLine("Last block hash is not valid");
            //         }

            //if (blockchain.LastBlockHashValid(block) && blockchain.BlockCountValid(block))
            //{
            //	blockchain.AddBlock(block);
            //}
            //=================================================================================
            //var blockchain = new Blockchain();
            //var pool = new TransactionPool();

            //var alice = new Wallet();
            //var bob = new Wallet();
            //var exchange = new Wallet();
            //var forger = new Wallet();

            //var exchangeTransaction = exchange.CreateTransaction(alice.GetPublicKeyString(),
            //	10, "EXCHANGE");

            //if (!pool.TransactionExist(exchangeTransaction))
            //{
            //	pool.AddTransaction(exchangeTransaction);
            //}

            //var coveredTransaction = blockchain.GetCoveredTransactionSet(pool._transactions);
            //var lastHash = BlockchainUtils.Hash(blockchain._blocks[blockchain._blocks.Count - 1].Payload());
            //var blockCount = blockchain._blocks[blockchain._blocks.Count - 1]._blockCount + 1;
            //var blockOne = new Block(coveredTransaction, Convert.ToBase64String(lastHash), forger.GetPublicKeyString(), blockCount);

            //blockchain.AddBlock(blockOne);
            //pool.RemoveFromPool(blockOne._transactions);
            ////Alice want to send 5 token to Bob

            //var transaction = alice.CreateTransaction(bob.GetPublicKeyString(), 5, "TRANSFER");

            //         if (!pool.TransactionExist(transaction))
            //         {
            //             pool.AddTransaction(transaction);
            //         }
            //         var coveredTransaction2 = blockchain.GetCoveredTransactionSet(pool._transactions);
            //         var lastHash2 = BlockchainUtils.Hash(blockchain._blocks[blockchain._blocks.Count - 1].Payload());
            //         var blockCount2 = blockchain._blocks[blockchain._blocks.Count - 1]._blockCount + 1;
            //         var blockTwo = new Block(coveredTransaction2, Convert.ToBase64String(lastHash2), forger.GetPublicKeyString(), blockCount2);

            //blockchain.AddBlock(blockTwo);

            //Console.WriteLine(blockchain.toJson());
            //string ip = args[0];
            //string port = args[1];
            string ip = "127.0.0.1";
            string port = "10015";
            Console.WriteLine($"Ip = {ip}; Port = {port}");
            //SocketCommunication.SocetConnectorInit(ip, port);
            
            //var node = new Node(ip, port);
            //Task startP2p = new Task(() => node.StartP2P());
            //startP2p.Start();
            //SocketCommunication.StartSocketCommunication(port);
            Console.WriteLine("Start...");
            Console.ReadLine();
        }
	}
}
