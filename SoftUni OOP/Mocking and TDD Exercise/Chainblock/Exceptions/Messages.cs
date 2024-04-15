using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chainblock.Exceptions
{
    public static class Messages
    {
        public static string DuplicateId = "Id has already been added!";
        public static string IncorrectId = "Invalid transaction id!";
        public static string IncorrectStatus = "Invalid transaction status!";
        public static string IncorrectSender = "Invalid sender input!";
        public static string IncorrectReceiver = "Invalid receiver input!";
        public static string NoSenderWithSuchTransactionAmount = "No sender with given transaction amount found!";
        public static string NoReceiverWithSuchTransactionAmount = "No receiver with given transaction amount found!";
    }
}
