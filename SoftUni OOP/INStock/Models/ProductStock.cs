using INStock.Contracts;
using INStock.Exceptions.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INStock.Models
{
    public class ProductStock : IProductStock, IEnumerable
    {
        IList<IProduct> products;

        public ProductStock()
        {
            products = new List<IProduct>();
        }

        public IProduct this[int index] 
        {
            get
            {
                try
                {
                    return products[index];
                }
                catch (Exception)
                {
                    throw new ArgumentOutOfRangeException(Messages.indexOutOfRange);
                }

               
            }
            
            set
            {
                try
                {
                   products[index] = value;
                }
                catch (Exception)
                {
                    throw new ArgumentOutOfRangeException(Messages.indexOutOfRange);
                }
            }
        } // DONE

        public int Count => products.Count; // DONE

        public void Add(IProduct product) // DONE
        {
            if (Contains(product))
            {
                throw new InvalidOperationException(Messages.invalidLabel);
            }

            products.Add(product);
        }

        public bool Contains(IProduct product) => products.Any(x => x.Label == product.Label); // DONE

        public IProduct Find(int index) => products[index]; // DONE

        public IEnumerable<IProduct> FindAllByPrice(decimal price) => products.Where(x => x.Price == price); // DONE

        public IEnumerable<IProduct> FindAllByQuantity(int quantity) => products.Where(x => x.Quantity == quantity);

        public IEnumerable<IProduct> FindAllInPriceRange(decimal lo, decimal hi) => products.Where(x => x.Price >= lo && x.Price <= hi)
                                                                                            .OrderByDescending(x => x.Price); // DONE

        public IProduct FindByLabel(string label) // DONE 
        {
            IProduct result = products.FirstOrDefault(x => x.Label == label);

            if ( result is null)
            {
                throw new ArgumentException(Messages.noLabelFopund);
            }

            return result;
        }

        public IProduct FindMostExpensiveProduct() => products.OrderByDescending(x => x.Price).FirstOrDefault();

        public bool Remove(IProduct product)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IProduct> GetEnumerator()
        {
            return products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
