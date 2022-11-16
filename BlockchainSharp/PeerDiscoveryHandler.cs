using BlockchainSharp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public static class PeerDiscoveryHandler
    {
        public static async Task Start()
        {
            Task statusThread = new Task(() => Status());
            statusThread.Start();
            Task discoveryThread = new Task(() => Discovery());
            discoveryThread.Start();
        }
        public static void Status()
        {
            while (true)
            {
                Console.WriteLine("Current Connection:");
                foreach (var peer in SocketCommunication._peers)
                {
                    Console.WriteLine($"{peer._ip} : {peer._port}");
                }
                Thread.Sleep(10000);
            }
            
        }
        public static async Task Discovery()
        {
            Mutex mutex1 = new();
            while (true)
            {
                bool ismutex = mutex1.WaitOne();
                string handshakeMessage = HandshakeMessage();
                SocketCommunication.Broadcast(handshakeMessage);
                Console.WriteLine($"Discovery Alive...");
                mutex1.ReleaseMutex();
                Thread.Sleep(10000);
            }
        }
        public static string HandshakeMessage()
        {
            var ownConnector = SocketCommunication._socketConnector;
            var data = SocketCommunication._peers;
            string messageType = "DISCOVERY";
            var message = new Message(ownConnector, messageType, JsonConvert.SerializeObject(data));
            string encodingMessage = JsonConvert.SerializeObject(message);
            return encodingMessage;
        }

        public static void HandleMessage(Message message)
        {
            var peersSocketConnector = message._senderConnector;
            var peersPeerList = BlockchainUtils.DeserializeData<List<SocketConnector>>(message);
            bool newPeer = true;
            foreach (var peer in SocketCommunication._peers)
            {
                if (peer.Equals(peersSocketConnector))
                {
                    newPeer = false;
                } 
            }
            if (newPeer)
            {
                SocketCommunication._peers.Add(peersSocketConnector);
            }
            foreach (var peersPeer in peersPeerList)
            {
                bool peerKnown = false;
                foreach (var peer in SocketCommunication._peers)
                {
                    if (peer.Equals(peersPeer))
                    {
                        peerKnown = true;
                    }
                }
                if (!peerKnown && !peersPeer.Equals(SocketCommunication._socketConnector))
                {
                    SocketCommunication.ConnectWithNode(peersPeer._ip, peersPeer._port);
                }
            }

        }
    }
}
