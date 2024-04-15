using Chainblock.Models;
using NUnit.Framework;
using System.Transactions;
using Transaction = Chainblock.Models.Transaction;
using TransactionStatus = Chainblock.Enums.TransactionStatus;

namespace CChainblock_Tests
{
    public class Tests
    {
        Transaction transaction;

        [SetUp]
        public void Setup()
        {
            transaction = new Transaction(1, TransactionStatus.Failed, "Pesho", "Gosho", 2.5m);

        }

        [Test]
        public void ConstructorShouldWork()
        {
            Assert.IsNotNull(transaction);
        }

        [Test]
        public void ConstructorShouldSetId()
        {
            Assert.AreEqual(1, transaction.Id);
        }

        [Test]
        public void ConstructorShouldSetTransactionStatus()
        {
            Assert.AreEqual(TransactionStatus.Failed, transaction.Status);
        }

        [Test]
        public void ConstructorShouldSetFromStatus()
        {
            Assert.AreEqual("Pesho", transaction.From);
        }

        [Test]
        public void ConstructorShouldSetToStatus()
        {
            Assert.AreEqual("Gosho", transaction.To);
        }

        [Test]
        public void ConstructorShouldSetAmount()
        {
            Assert.AreEqual(2.5, transaction.Amount);
        }

    }
}