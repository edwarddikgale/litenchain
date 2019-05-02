using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Litenchain
{
    public interface IPeer2PeerClient
    {
        void Connect(string url);
        void Send(string url, string data);
        void Close();
        void Broadcast(string data);
        IList<string> GetServers();
    }

    public class Peer2PeerClient: IPeer2PeerClient
    {
        protected internal IDictionary<string, WebSocket> webSockets = new Dictionary<string, WebSocket>();

        public Peer2PeerClient()
        {
        }

        public void Broadcast(string data)
        {
            foreach (var ws in webSockets)
            {
                ws.Value.Send(data);
            }
        }

        public void Close()
        {
            foreach(var ws in webSockets)
            {
                ws.Value.Close();
            }
        }

        public void Connect(string url)
        {
            Console.WriteLine($"Will now connect to server:{url}");

            //if no connection already exists
            if (!webSockets.ContainsKey(url))
            {
                WebSocket ws = new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        Blockchain proposedChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (proposedChain.IsValid() && proposedChain.Chain.Count > Program.LitenCoin.Chain.Count)
                        {
                            List<Transaction> txs = new List<Transaction>();
                            txs.AddRange(proposedChain.PendingTransactions);
                            txs.AddRange(Program.LitenCoin.PendingTransactions);

                            proposedChain.PendingTransactions = txs;
                            Program.LitenCoin = proposedChain;
                        }

                    }
                };

                ws.Connect();
                
                //ws.Send("Hi Server");
                Console.WriteLine("Sending chain to subscribers");
                ws.Send(JsonConvert.SerializeObject(Program.LitenCoin));

                webSockets.Add(url, ws);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> list = new List<string>();
            foreach (var ws in webSockets)
                list.Add(ws.Key);

            return list;
        }

        public void Send(string url, string data)
        {
            foreach(var item in webSockets){
                if(item.Key == url){
                    item.Value.Send(data);
                }
            }
        }
    }
}
