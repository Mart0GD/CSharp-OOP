using INStock.Models;
using System;

namespace INStock
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            ProductStock stock = new ProductStock();

            Product bear = new("Mechi", 3.5m, 10);
            Product panda = new("Belo mechi", 5m, 10);
            Product grizzli = new("Puffy Mechi", 3.5m, 10);

            stock.Add(bear);
            stock.Add(bear);
            stock.Add(panda);
            stock.Add(grizzli);

            Console.WriteLine(stock.FindMostExpensiveProduct());
            Console.WriteLine(string.Join(" ", stock.FindAllByQuantity(10)));
            Console.WriteLine(string.Join(" ", stock.FindAllByPrice(3.5m)));
        }
    }
}
