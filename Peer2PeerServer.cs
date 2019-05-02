using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Litenchain
{
    public class Peer2PeerServer : WebSocketBehavior
    {
        private bool ChainSynched { get; set; }
        private WebSocketServer Wss { get; set; }

        public void Start()
        {
            Console.WriteLine($"Starting server on port ... {Program.Port}");
            Wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            
            Wss.AddWebSocketService<Peer2PeerServer>("/litenchain");
            Wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Wss.Port}");
            Console.WriteLine($"Server is listening? {Wss.IsListening}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                Console.WriteLine("Attempting to receive chain from connecting client");
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                //if (newChain.IsValid() && )
                if(newChain.Chain.Count > Program.LitenCoin.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.LitenCoin.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.LitenCoin = newChain;
                }

                if (!ChainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.LitenCoin));
                    ChainSynched = true;
                }
            }

        }
    }
}
