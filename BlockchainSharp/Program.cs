using BlockchainSharp;

namespace SharpChain
{
	class Program
	{
		static void Main(string[] args)
		{
			string sender = "sender";
			string receiver = "receiver";
			int amount = 100;
			string type = "TRANSFER";

			var wallet = new Wallet();
			var fraudFallet = new Wallet();
			var pool = new TransactionPool();

			var transaction = wallet.CreateTransaction(receiver, amount, type);



			string transactionPayload = transaction.Payload();
            
			bool signatureValid = Wallet.SignatureValid(transactionPayload, transaction._signature, wallet.GetPublicKeyString());
			
			if (signatureValid)
			{
				Console.WriteLine("transaction signature is valid");
			}



			if (!pool.TransactionExist(transaction))
				pool.AddTransaction(transaction);

			var blockchain = new Blockchain();

			byte[] lastHash = BlockchainUtils.Hash(blockchain._blocks[blockchain._blocks.Count - 1].Payload());
			int blockCount = blockchain._blocks[blockchain._blocks.Count - 1]._blockCount + 1;
			var block = wallet.CreateBlock(pool.transactions, lastHash.ByteArrayToString(), blockCount);

			if (!blockchain.LastBlockHashValid(block))
			{
				Console.WriteLine("Last block hash is not valid");
			}
            if (!blockchain.BlockCountValid(block))
            {
                Console.WriteLine("Last block hash is not valid");
            }

			if (blockchain.LastBlockHashValid(block) && blockchain.BlockCountValid(block))
			{
				blockchain.AddBlock(block);
			}

			Console.WriteLine(blockchain.toJson());
        }
	}
}
