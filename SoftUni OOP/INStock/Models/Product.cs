using INStock.Contracts;
using INStock.Exceptions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INStock.Models
{
    public class Product : IProduct
    {
        private string label;
        private decimal price;
        private int quantity;

        public Product(string label, decimal price, int quantity)
        {
            Label = label;
            Price = price;
            Quantity = quantity;
        }

        public string Label
        {
            get => label;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(Messages.nullOrWhiteSpace);
                }

                label = value;
            }
        }

        public decimal Price
        {
            get => price;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(Messages.negative);
                }

                price = value;
            }
        }

        public int Quantity
        {
            get => quantity;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(Messages.quantity);
                }

                quantity = value;
            }
        }

        public int CompareTo(IProduct other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.Label + " " + this.Price;
        }
    }
}
