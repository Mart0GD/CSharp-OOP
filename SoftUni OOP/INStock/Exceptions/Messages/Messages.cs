using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INStock.Exceptions.Messages
{
    public static class Messages
    {
        public static string nullOrWhiteSpace = "Input should not be null or whitespace!";
        public static string negative = "Input should not be negative!";
        public static string quantity = "Quantity should not be negative or zero!";
        public static string indexOutOfRange = "Index is out of range!";
        public static string noLabelFopund = "Invalid label! No item with given label found";
        public static string invalidLabel = "Inalid label! Label has already been added";

    }
}
