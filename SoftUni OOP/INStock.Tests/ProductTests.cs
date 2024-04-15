using INStock.Exceptions.Messages;
using INStock.Models;
using NUnit.Framework;
using System;

namespace INStock.Tests
{
    public class ProductTests
    {
        Product product;

        [SetUp]
        public void SetUp()
        {
            product = new("drink", 15, 2);
        }

        [Test]
        public void ConstructorShuldNotBeNull()
        {
            Assert.IsNotNull(product);
        }

        [Test]
        public void LabelSetterShouldSetLabelValue()
        {
            Assert.AreEqual("drink", product.Label);
        }

        [TestCase(" ")]
        [TestCase("         ")]
        [TestCase(null)]
        public void LabelShouldThrowExceptionIfLabelIsNullOrWhitepace(string input)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Product(input, 109, 1));

            Assert.AreEqual(ex.Message, Messages.nullOrWhiteSpace);
        }

        [Test]
        public void PriceSetterShouldSetPriceValue()
        {
            Assert.AreEqual(15, product.Price);
        }

        [TestCase(-101.0202932)]
        [TestCase(-1)]
        public void PriceShouldThrowExceptionIfPriceIsNegative(decimal input)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Product("Kuche", input, 1));

            Assert.AreEqual(ex.Message, Messages.negative);
        }

        [Test]
        public void QuantitySetterShouldSetQuantityValue()
        {
            Assert.AreEqual(2, product.Quantity);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void QuantityShouldThrowExceptionIfQuantityIsNegativeOrZero(int input)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Product("Kuche", 12, input));

            Assert.AreEqual(ex.Message, Messages.quantity);
        }
    }
}
