using System;

namespace Litenchain
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public Double Amount { get; set; }

        public Transaction(string from, string to, double amount){
            Sender = from;
            Receiver = to;
            Amount = amount;
        }   

        public Transaction(){}
    }
}