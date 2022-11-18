using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlockchainSharp
{
    public class Node : IHostedService, IDisposable
    {
        private readonly ILogger<Node> _logger;
        public string _hostAddress { get; set; }
        public string _ip { get; set; }
        public string _port { get; set; }
        public NetMQActor _p2p { get; set; }
        public Blockchain _blockchain { get; set; }
        public TransactionPool _transactionPool { get; set; }
        public Wallet _wallet { get; set; }

        public Node(ILogger<Node> logger)
        {
            
            _blockchain = new Blockchain();
            _transactionPool = new TransactionPool();
            _wallet = new Wallet();
            _p2p = null;
            _ip = "127.0.0.1";
            _port = "5001";
            _logger = logger;
        }

        public void RecivedTransaction(string transaction)
        {
            Message deserializedMessage;
            deserializedMessage = JsonConvert.DeserializeObject<Message>(transaction);
            if (deserializedMessage?._messageType == "TRANSACTION")
            {
                Console.WriteLine(deserializedMessage._data);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _p2p = Bus.Create(int.Parse(_port));
            _p2p.SendFrame(Bus.GetHostAddressCommand);
            _hostAddress = _p2p.ReceiveFrameString();

            Thread.Sleep(2100);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    string message = _p2p.ReceiveFrameString();

                    switch (message)
                    {
                        case Bus.AddedNodeCommand:
                            var addedAddress = _p2p.ReceiveFrameString();
                            Console.WriteLine("Added node {0} to the Bus", addedAddress);
                            break;
                        case Bus.RemovedNodeCommand:
                            var removedAddress = _p2p.ReceiveFrameString();
                            Console.WriteLine("Removed node {0} from the Bus", removedAddress);
                            break;
                        case Bus.Transaction:
                            var messageTransaction = _p2p.ReceiveFrameString();
                            var msg = JsonConvert.DeserializeObject<Message>(messageTransaction);
                            var trn = JsonConvert.DeserializeObject<Transaction>(msg._data);
                            if (trn is Transaction)
                                HandleTransaction(trn);
                            break;
                        default:
                            Console.WriteLine(message);
                            break;
                    }
                    Thread.Sleep(3000);
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blockchain Node Stopping");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public bool HandleTransaction(Transaction transaction)
        {
            Console.WriteLine($"Received transaction {transaction.toJson()}");
            string data = transaction.Payload();
            string signature = transaction._signature;
            string signerPublicKey = transaction._senderPublicKey;
            bool transactionExist = _transactionPool.TransactionExist(transaction);
            if (transactionExist)
            {
                Console.WriteLine("Transaction Exist");
                return false;
            }
            bool signatureValid = Wallet.SignatureValid(data, signature, signerPublicKey);
            if (!signatureValid)
            {
                Console.WriteLine("Signature invalid");
                return false;
            }
            _transactionPool.AddTransaction(transaction);
            string message = Message.CreateMessage(_hostAddress, "TRANSACTION", transaction.toJson());
            _p2p.SendMoreFrame(Bus.PublishCommand).SendMoreFrame(Bus.Transaction).SendFrame(message);
            return true;
        }
    }
}
