using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Litenchain
{
    public class Block
    {
        public int Index{get;set;}

        public int Nonce{get;set;}
        public DateTime TimeStamp{get;set;}
        public string PreviousHash{get;set;}
        public string Hash{get;set;}
        public List<Transaction> Data{get;set;}

        public Block(DateTime timeStamp, string prevHash, List<Transaction> data){
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = prevHash;
            Data = data;        
            Hash = CalculateHash();
        }

        public string CalculateHash(){
            
            SHA256 sha256 = SHA256.Create();
            string prevHash = PreviousHash ?? "";
            string data = JsonConvert.SerializeObject(Data);

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{prevHash} - {data} - {Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty){

            var maxCount = 5000;
            var worker = 0;
            var leadingZeros = new string('0', difficulty);
            while(this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros || ++worker < maxCount){

                Console.ForegroundColor = ConsoleColor.Green;

                this.Nonce++;
                this.Hash = this.CalculateHash();

                Console.WriteLine($"Attempting to find appropriate hash");
                Console.WriteLine($"\r{this.Hash}");

            }

            Console.WriteLine($"MINER successfully mined block with hash: {this.Hash}");

            Console.ResetColor();
            
        }
        
    }
}