using INStock.Contracts;
using INStock.Exceptions.Messages;
using INStock.Models;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace INStock.Tests
{
    public class ProductStockTests
    {
        ProductStock stock;
        Product product;
        Random random;

        [SetUp]
        public void SetUp()
        {
            stock = new ProductStock();
            product = new("Ketchap", 4.50m, 1);
            random = new Random();
        }

        [Test]
        public void ConstructorShouldNotBeNull()
        {
            Assert.IsNotNull(stock);
        }


        [Test]
        public void AddShouldIncreaseCount()
        {
            stock.Add(product);

            Assert.AreEqual(1, stock.Count);
        }

        [Test]
        public void AddShouldThrowExceptionIfLabelAlredyAdded()
        {
            stock.Add(product);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => stock.Add(product));

            Assert.AreEqual(Messages.invalidLabel, ex.Message);
        }


        [Test]
        public void CountainsShouldReturnTrueWhenItemExists()
        {
            stock.Add(product);

            Assert.IsTrue(stock.Contains(product));
        }

        [Test]
        public void CountainsShouldReturnFalseWhenItemDoesNotExist()
        {
            Assert.IsFalse(stock.Contains(product));
        }

        [TestCase(2)]
        public void CountShouldRetunProductsCount(int itemsToAdd)
        {
            for (int i = 0; i < itemsToAdd; i++)
            {
                stock.Add(new Product(random.Next().ToString(), random.Next(), random.Next()));
            }

            Assert.AreEqual(itemsToAdd, stock.Count);
        }

        [Test]
        public void IndexerShouldWorkProperly()
        {
            stock.Add(product);

            Assert.AreEqual(stock[0], product);
        }

        [Test]
        public void IndexerShouldThrowExceptionIfGetIndexIsOutOfRange()
        {
            IProduct test = default(IProduct);

            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => test = stock[1]);

            Assert.AreEqual(Messages.indexOutOfRange, ex.ParamName);
        }


        [Test]
        public void IndexerShouldThrowExceptionIfSetIndexIsOutOfRange()
        {
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => stock[1] = product);

            Assert.AreEqual(Messages.indexOutOfRange, ex.ParamName);
        }

        [Test]
        public void FindShouldReturnTheNthAddedProduct()
        {
            IProduct productOne = new Product("penguin", 3.50m, 10);
            IProduct productTwo = new Product("gosho", 6m, 10);

            stock.Add(product);
            stock.Add(productTwo);
            stock.Add(productOne);

            Assert.AreEqual(productOne, stock.Find(2));
        }

        [Test]
        public void FindByLabelShouldReturnProductWithGivenLabel()
        {
            stock.Add(product);
            stock.Add(new Product("penguin", 3.50m, 10));

            Assert.AreEqual("penguin", stock.FindByLabel("penguin").Label);
        }

        [Test]
        public void FindByLabelShouldShouldThrowExceptionNoLabelWasFound()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => stock.FindByLabel("penguin"));

            Assert.AreEqual(Messages.noLabelFopund, ex.Message);
        }

        [TestCase(3.00, 4.00)]
        public void FindAllInPriceRangeShouldReturnAllProductsWithinTheRange(decimal lower, decimal higher)
        {
            List<IProduct> expected = new() { new Product("penguin", 3.50m, 10), new Product("gosho", 4.00m, 10) };

            stock.Add(product);
            stock.Add(expected[0]);
            stock.Add(expected[1]);

            List<IProduct> found = stock.FindAllInPriceRange(lower, higher).ToList();

            CollectionAssert.AreEqual(expected.OrderByDescending(x => x.Price), found);
        }

        [TestCase(6.0, 7.00)]
        public void FindAllInPriceRangeShouldReturnEmptyEnumerationIfNoProductsFound(decimal lower, decimal higher)
        {
            IEnumerable<IProduct> found = stock.FindAllInPriceRange(lower, higher);

            Assert.IsTrue(found is IEnumerable<IProduct> && found.Count() == 0);
        }

        [TestCase(3.50)]
        public void FindAllByPriceShouldReturnAllProductsWithGivenPrice(decimal price)
        {
            List<IProduct> expected = new() { new Product("penguin", 3.50m, 10), new Product("gosho", 3.50m, 10) };

            stock.Add(product);
            stock.Add(expected[0]);
            stock.Add(expected[1]);

            CollectionAssert.AreEqual(expected, stock.FindAllByPrice(price).ToList());
        }

        [TestCase(8.50)]
        public void FindAllByPriceShouldReturnEmptycollectionIfNoProductsAreFound(decimal price)
        {
            stock.Add(product);

            var found = stock.FindAllByPrice(price);

            Assert.IsTrue(found is IEnumerable<IProduct> && found.Count() == 0);
        }

        [Test]
        public void FindMostExpensiveProductShouldRetunrTheProductWithLargestPrice()
        {
            IProduct productOne = new Product("penguin", 3.50m, 10);
            IProduct productTwo = new Product("gosho", 6m, 10);

            stock.Add(productOne);
            stock.Add(productTwo);

            Assert.AreEqual(productTwo, stock.FindMostExpensiveProduct());
        }

        [TestCase(8.50)]
        public void FindMostExpensiveProductShouldReturnEmptyIfNoProductFound(decimal price)
        {
            var found = stock.FindMostExpensiveProduct();

            Assert.IsTrue(found is null);
        }

        [TestCase(10)]
        public void FindAllByQuantityShouldReturnAllProductsWithGivenQuantity(int quantity)
        {
            IProduct productOne = new Product("penguin", 3.50m, 10);
            IProduct productTwo = new Product("gosho", 6m, 10);

            stock.Add(productOne);
            stock.Add(productTwo);

            List<IProduct> found = stock.FindAllByQuantity(quantity).ToList();

            Assert.AreEqual(productOne, found[0]);
            Assert.AreEqual(productTwo, found[1]);
        }


        [TestCase(13)]
        public void FindAllByQuantityShouldReturnEmptyEnumerationWhenNoProductsWithGivenQuantityAreFound(int quantity)
        {
            IProduct productOne = new Product("penguin", 3.50m, 10);
            IProduct productTwo = new Product("gosho", 6m, 10);

            stock.Add(productOne);
            stock.Add(productTwo);

            List<IProduct> found = stock.FindAllByQuantity(quantity).ToList();

            Assert.IsTrue(found is IEnumerable<IProduct> && found.Count() == 0);
        }

        [Test]
        public void GetEnumeratorReturnAllProductsInStock()
        {
            stock.Add(product);

            int count = 0;
            foreach (var item in stock)
            {
                count++;
            }

            Assert.AreEqual(count, stock.Count);
        }
    }
}
