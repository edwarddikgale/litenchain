using System;
using Newtonsoft.Json;

namespace Litenchain
{
    public class Program
    {
        public static int Port { get; internal set; }
        public static Blockchain LitenCoin { get; internal set; }

        static void LoadChain(){
            LitenCoin = new Blockchain();
        }

        static void Main(string[] args)
        {
            LoadChain();
            //Port = 8181;
            if (args != null && args.Length > 0) Port = int.Parse(args[0]);
            Console.WriteLine("Welcome to Litenchain. State port to start server on: ");

            Port = int.Parse(Console.ReadLine());
            var server = new Peer2PeerServer();
            var client = new Peer2PeerClient();

            server.Start();

            var quit = "10";
            var instructions = $"0 = help, 1 = Connect to server, 2 = Add Tx, 3 = Display chain, 4 = Broadcast, 5 = Mine, {quit} = Quit";
            Console.WriteLine(instructions);

            var inp = Console.ReadLine();
            
            while(inp != quit){   

                if(inp == "0") Console.WriteLine(instructions);             

                if (inp == "1")
                {
                    Console.WriteLine("url? ");
                    var url = Console.ReadLine();
                    Console.WriteLine($"Attempting connection to {url}");
                    client.Connect(url);
                }
                
                if(inp == "2"){
                    var tx = new Transaction();
                    Console.WriteLine("From:");
                    tx.Sender = Console.ReadLine();
                    Console.WriteLine("To:");
                    tx.Receiver = Console.ReadLine();
                    Console.WriteLine("Amount:");
                    tx.Amount = Double.Parse(Console.ReadLine());
                    
                    Console.WriteLine($"Added tx: {JsonConvert.SerializeObject(tx)} ");

                    Program.LitenCoin.PendingTransactions.Add(tx);
                }

                if(inp == "3"){
                    var currChain = JsonConvert.SerializeObject(Program.LitenCoin);
                    Console.WriteLine($"{currChain}");
                }

                if(inp == "4"){
                    Console.WriteLine("Broadcasting chain to subscribers");                     
                    client.Broadcast(JsonConvert.SerializeObject(Program.LitenCoin));
                }

                if(inp == "5"){
                    Console.WriteLine("Processing pending tx");
                    Program.LitenCoin.ProcessTxPending();
                }

                inp = Console.ReadLine();
            }
                
        }
    }
}
