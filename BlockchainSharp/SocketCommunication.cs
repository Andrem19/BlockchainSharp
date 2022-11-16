using BlockchainSharp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NetMQ.Sockets;
using NetMQ;

namespace BlockchainSharp
{
    public static class SocketCommunication
    {
        static public List<SocketConnector> _peers { get; set; } = new List<SocketConnector>();
        static public SocketConnector _socketConnector { get; set; } 

        public static void SocetConnectorInit(string ip, string port)
        {
            _socketConnector = new SocketConnector(ip, port);
        }
        public static async Task ConnectToFirstNode(string port)
        {
            if (port != "5001")
                ConnectWithNode("127.0.0.1", "5001");
        }
        public static async Task StartSocketCommunication(string port)
        {
            try
            {
                Task startNode = new Task(() => Start(port));
                startNode.Start();
                Task status = new Task(() => PeerDiscoveryHandler.Status());
                status.Start();
                Task discovery = new Task(() => PeerDiscoveryHandler.Discovery());
                discovery.Start();
                Task connectToFirstNode = new Task(() => ConnectToFirstNode(port));
                connectToFirstNode.Start();
                Task.WaitAll(startNode, connectToFirstNode, discovery, status);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        
        public static void NodeMessage(string message)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(message);
            if (msg._messageType == "DISCOVERY")
                PeerDiscoveryHandler.HandleMessage(msg);
        }
        public static void Broadcast(string message)
        {
            SendToNodes(message);
        }

        public static async Task Start(string port)
        {
            try
            {
                using (var receiver = new DealerSocket())
                {
                    string receivedMessage;
                    receiver.Bind($"tcp://*:{port}");
                    while (true)
                    {
                        TimeSpan timeout = new TimeSpan(0, 0, 2);
                        var message = receiver.TryReceiveFrameString(timeout, out receivedMessage);

                        if (!string.IsNullOrEmpty(receivedMessage))
                        {
                            Console.WriteLine($"Message recived {receivedMessage}");
                            NodeMessage(receivedMessage);
                            receiver.TrySendFrame($"{_socketConnector._ip}:{_socketConnector._port}");
                        }
                        //Thread.Sleep(2000);
                        //Console.WriteLine($"Server{port} Alive...");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Start Node Connection Method");
            }
        }

        public static void SendToNode(SocketConnector receiver, string message)
        {
            try
            {
                using (var sender = new DealerSocket())
                {
                    string receivedMessage;
                    sender.Connect($"tcp://{receiver._ip}:{receiver._port}");

                    var msg = PeerDiscoveryHandler.HandshakeMessage();
                    sender.SendFrame(msg);
                    sender.TryReceiveFrameString(out receivedMessage);
                    Console.WriteLine($"Message delivered {receivedMessage}");
                }
            }
            catch (Exception)
            {

                throw new Exception("Send To Node Method");
            }
            
        }
        public static void SendToNodes(string message)
        {
            try
            {
                foreach (var peer in _peers)
                {

                    using (var sender = new DealerSocket())
                    {
                        sender.Connect($"tcp://{peer._ip}:{peer._port}");
                        string receivedMessage;
                        var msg = message;
                        sender.TrySendFrame(msg);
                        TimeSpan timeout = new TimeSpan(0, 0, 1);
                        bool isMessage = sender.TryReceiveFrameString(timeout, out receivedMessage);
                        if (!isMessage)
                        {
                            Mutex mutex3 = new Mutex();
                            mutex3.WaitOne();
                            bool isRemoves = _peers.Remove(peer);
                            if (isRemoves)
                                Console.WriteLine($"Peer {peer._ip}:{peer._port} leave us...");
                            mutex3.ReleaseMutex();
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw new Exception("Send To Nodes Method");
            }
        }
        public static void ConnectWithNode(string ip, string port)
        {
            try
            {
                Console.WriteLine("Connect With First Node Called");
                //var message = PeerDiscoveryHandler.HandshakeMessage();
                var mes = new Message(_socketConnector, "HANDSHAKE", "Hi");
                string message = JsonConvert.SerializeObject(mes);
                using (var sender = new DealerSocket())
                {
                    sender.Connect($"tcp://{ip}:{port}");
                    string receivedMessage;
                    sender.TrySendFrame(message);
                    Console.WriteLine($"Messaage sent: {message}");
                    TimeSpan timeout = new TimeSpan(0, 0, 1);
                    bool isMessage = sender.TryReceiveFrameString(timeout, out receivedMessage);
                    if (isMessage)
                    {
                        Mutex mutex2 = new();
                        bool ismutex = mutex2.WaitOne();
                        SocketConnector newPeer = new SocketConnector(ip, port);
                        _peers.Add(newPeer);
                        mutex2.ReleaseMutex();
                        Console.WriteLine($"Connection esteblished {receivedMessage}");
                    }
                    else
                    {
                        if (_peers.Exists(x => x._ip == ip && x._port == port))
                        {
                            var peer = _peers.FirstOrDefault(x => x._ip == ip && x._port == port);
                            _peers.Remove(peer);
                        }
                        Console.WriteLine("ConnectWithNode: message was null or empty");
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Connect With Node Method");
            }

        }
    }
}
