using Chainblock.Contracts;
using Chainblock.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Chainblock.Models
{
    public class Transaction : ITransaction
    {
        //private int id;
        //private TransactionStatus status;
        //private string from;

        public Transaction(int id, TransactionStatus status, string from, string to, decimal amount)
        {
            Id = id;
            Status = status;
            From = from;
            To = to;
            Amount = amount;
        }

        public int Id { get; set; }
        public TransactionStatus Status { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
    }
}
