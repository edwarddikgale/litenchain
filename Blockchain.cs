using System;
using System.Collections.Generic;

namespace Litenchain
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> PendingTransactions { get; set; }
        public int Difficulty{get ;set; } = 1;
        public double MiningAward = 1;

        public Blockchain(){
            InitialiseChain();
        }

        public void InitialiseChain(){
            Chain = new List<Block>();
            AddGenesisBlock();
            PendingTransactions = new List<Transaction>();
        }

        public Block CreateGenesisBlock(){
            return new Block(DateTime.Now, null, null);
        }

        public void AddGenesisBlock(){
            Chain.Add(CreateGenesisBlock());
        }

        public void AddTx(Transaction tx){
            PendingTransactions.Add(tx);
        }

        public void ProcessTxPending(){

            var block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            //clear out pending tx list
            PendingTransactions = new List<Transaction>();

            //award the miner
            AddTx(new Transaction("Litenchain","Alex", MiningAward));
        }

        public Block GetLatestBlock(){
            return Chain[Chain.Count - 1];
        }

        //A block is only added through mining
        public void AddBlock(Block block){
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;

            //block.Hash = block.CalculateHash();
            block.Mine(this.Difficulty);

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
