using System;
using System.Collections.Generic;

namespace Litenchain
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> PendingTransactions { get; set; }

        public Blockchain(){
            InitialiseChain();
        }

        public void InitialiseChain(){
            Chain = new List<Block>();
            PendingTransactions = new List<Transaction>();
        }

        public Block CreateGenesisBlock(){
            return new Block(DateTime.Now, null, null);
        }

        public void AddGenesisBlock(){
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock(){
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block){
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }

        internal bool IsValid()
        {
            /*
            for(int i  = 1; i < Chain.Count; i++){
                Block prevBlock = Chain[i - 1];
                Block currBlock = Chain[i];

                if(currBlock.PreviousHash != prevBlock.Hash) return false;

                if(currBlock.Hash != currBlock.CalculateHash()) return false;
            } */

            return true;
        }
    }
}
