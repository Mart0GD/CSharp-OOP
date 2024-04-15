using Chainblock.Contracts;
using Chainblock.Enums;
using Chainblock.Exceptions;
using Chainblock.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction = Chainblock.Models.Transaction;

namespace CChainblock_Tests;

public class ChainblockTests
{
    Chainclock chainblock;
    Transaction transaction;
    Transaction transactionTwo;
    List<ITransaction> transactions;

   [SetUp]
    public void SetUp()
    { 
        chainblock = new();
        transaction = new Transaction(10,TransactionStatus.Successfull,"Pesho","Gosho",10.5m);
        transactionTwo = new Transaction(1000, TransactionStatus.Successfull, "Pesho", "Gosho", 8.00m);

        transactions = new List<ITransaction>() { transaction, transactionTwo};
    }

    [Test]
    public void ConstructorShouldWork()
    {
        Assert.IsNotNull(chainblock);
    }

    [Test]
    public void AddMethodShouldIncreaseCount()
    {
        chainblock.Add(transaction);

        Assert.AreEqual(1, chainblock.Count);
    }

    [Test]
    public void AddShouldThrowExceptionWhenSameIdIsAdded()
    {
        chainblock.Add(transaction);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.Add(transaction));

        Assert.AreEqual(Messages.DuplicateId, ex.Message);
    }

    [Test]
    public void ContainsIdShouldReturnTrueWhenTransactionIdExists()
    {
        chainblock.Add(transaction);

        Assert.IsTrue(chainblock.Contains(10));
    }

    [Test]
    public void ContainsIdShouldReturnFalseWhenTransactionIdDoesNotExists()
    {
        Assert.IsFalse(chainblock.Contains(10));
    }

    [Test]
    public void ContainsShouldReturnTrueWhenTransactionExists()
    {
        chainblock.Add(transaction);

        Assert.IsTrue(chainblock.Contains(transaction));
    }

    [Test]
    public void ContainsShouldReturnFalseWhenTransactionDoesNotExists()
    {
        Assert.IsFalse(chainblock.Contains(transaction));
    }


    [Test]
    public void ChangeTransactionStatusShouldChangeTheGiveTransactionStatus()
    {
        chainblock.Add(transaction);

        chainblock.ChangeTransactionStatus(transaction.Id, TransactionStatus.Failed);

        Assert.AreEqual(TransactionStatus.Failed, transaction.Status);
    }

    [Test]
    public void ChangeTransactionStatusShouldThrowExceptionIfIdDoesNotExist()
    {
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.ChangeTransactionStatus(transaction.Id, TransactionStatus.Failed));

        Assert.AreEqual(Messages.IncorrectId, ex.Message);
    }

    [Test]
    public void RemoveTransactionByIdShouldRemoveTheGiveTransaction()
    {
        chainblock.Add(transaction);

        chainblock.RemoveTransactionById(transaction.Id);

        Assert.AreEqual(0, chainblock.Count);
    }

    [Test]
    public void RemoveTransactionByIdShouldThrowExceptionIfIdDoesNotExist()
    {
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.RemoveTransactionById(transaction.Id));

        Assert.AreEqual(Messages.IncorrectId, ex.Message);
    }

    [Test]
    public void GetByIdShouldGiveTheReturnTransactionById()
    {
        chainblock.Add(transaction);

        Assert.AreEqual(transaction, chainblock.GetById(transaction.Id));
    }

    [Test]
    public void GetByIdShouldThrowExceptionIfIdDoesNotExist()
    {
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetById(transaction.Id));

        Assert.AreEqual(Messages.IncorrectId, ex.Message);
    }

    [Test]
    public void	GetByTransactionStatusShouldReturnAllTransactionsWithGivenStatus()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(transactions.OrderByDescending(x => x.Amount), chainblock.GetByTransactionStatus(TransactionStatus.Successfull));
    }

    [Test]
    public void GetByTransactionStatusShouldThrowExceptionIfNoTransactionsWithGivenIdExist()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetByTransactionStatus(TransactionStatus.Failed));

        Assert.AreEqual(Messages.IncorrectStatus, ex.Message);
    }

    [Test]
    public void GetAllSendersWithTransactionStatusShouldReturnAllSendersWithGivenStatus()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(transactions.OrderByDescending(x => x.Amount).Select(x => x.From), chainblock.GetAllSendersWithTransactionStatus(TransactionStatus.Successfull));
    }

    [Test]
    public void GetAllSendersWithTransactionStatusShouldThrowExceptionIfNoSendersWithTransactionsWithGivenIdExist()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetAllSendersWithTransactionStatus(TransactionStatus.Failed));

        Assert.AreEqual(Messages.IncorrectStatus, ex.Message);
    }

    [Test]
    public void GetAllReceiversWithTransactionStatusShouldReturnAllReceiversWithGivenStatus()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(transactions.OrderByDescending(x => x.Amount).Select(x => x.To), chainblock.GetAllReceiversWithTransactionStatus(TransactionStatus.Successfull));
    }

    [Test]
    public void GetAllReceiversWithTransactionStatusShouldThrowExceptionIfNoReceiversWithTransactionsWithGivenIdExist()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetAllReceiversWithTransactionStatus(TransactionStatus.Failed));

        Assert.AreEqual(Messages.IncorrectStatus, ex.Message);
    }

    [Test]
    public void GetAllOrderedByAmountDescendingThenByIdShouldReturnAllTransactionsOrderdByAmountdescendingThenById()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(transactions.OrderByDescending(x => x.Amount).ThenBy(x => x.Id), chainblock.GetAllOrderedByAmountDescendingThenById());
    }

    [Test]
    public void GetBySenderOrderedByAmountDescendingShouldReturnAllTransactionsWithGivenSenderOrderedByAmountdescending()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> {transaction, transactionTwo}.OrderByDescending(x => x.Amount) as IEnumerable<ITransaction>, chainblock.GetBySenderOrderedByAmountDescending("Pesho"));
    }

    [Test]
    public void GetBySenderOrderedByAmountDescendingShouldThrowExceptionIfNoTransactionsWithGivenSenderExist()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetBySenderOrderedByAmountDescending("Ceca Mecata"));

        Assert.AreEqual(Messages.IncorrectSender, ex.Message);
    }

    [Test]
    public void GetByReceiverOrderedByAmountThenByIdShouldReturnAllTransactionsWithGivenReceiverOrderedByAmountdescendingAndThenById()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> { transaction, transactionTwo }.OrderByDescending(x => x.Amount).ThenBy(x => x.Id) as IEnumerable<ITransaction>, chainblock.GetByReceiverOrderedByAmountThenById("Gosho"));
    }

    [Test]
    public void GetByReceiverOrderedByAmountThenByIdShouldThrowExceptionIfNoTransactionsWithGivenReceiverExist()
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetByReceiverOrderedByAmountThenById("Ceca Mecata"));

        Assert.AreEqual(Messages.IncorrectReceiver, ex.Message);
    }

    [TestCase(10)]
    [TestCase(8)]
    [TestCase(5)]
    public void GetByTransactionStatusAndMaximumAmountShouldReturnAllTransactionWithGivenStatusThatHaveLessAmountThenGiven(decimal amount)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> { transaction, transactionTwo }.Where(x => x.Amount <= amount).OrderByDescending(x => x.Amount) as IEnumerable<ITransaction>, chainblock.GetByTransactionStatusAndMaximumAmount(TransactionStatus.Successfull, amount));
    }

    [TestCase("Pesho", 10)]
    [TestCase("Pesho", 8)]
    [TestCase("Pesho", 5)]
    public void GetBySenderAndMinimumAmountDescendingShouldReturnAllTransactionsWithGivenSenderAndamountBiuggerThanTheGIvenAmount(string sender, decimal amount)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> { transaction, transactionTwo }.Where(x => x.Amount > amount && x.From == sender).OrderByDescending(x => x.Amount) as IEnumerable<ITransaction>, chainblock.GetBySenderAndMinimumAmountDescending(sender, amount));
    }

    [TestCase("Pesho", 22)]
    [TestCase("Pesho", 11)]
    [TestCase("cecoccc", 5)]
    public void GetBySenderAndMinimumAmountDescendingShouldThrowExceptionIfNoSendersWithBiggerAmountFound(string sender, decimal amount)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetBySenderAndMinimumAmountDescending(sender, amount));

        Assert.AreEqual(Messages.NoSenderWithSuchTransactionAmount, ex.Message);
    }

    [TestCase("Gosho", 8, 10)]
    [TestCase("Gosho", 7, 10)]
    public void GetByReceiverAndAmountRangeShouldReturnAllTransactionsWithGivenReceiverAndAmountBetweenGivenBoundries(string reciver, decimal low, decimal hi)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> { transaction, transactionTwo }.Where(x => x.To == reciver && x.Amount >= low && x.Amount < hi).OrderByDescending(x => x.Amount).ThenBy(x => x.Id) as IEnumerable<ITransaction>, chainblock.GetByReceiverAndAmountRange(reciver, low,hi));
    }

    [TestCase("Gosho", 13, 15)]
    [TestCase("Gosho", 10.7, 18)]
    [TestCase("cecoccc", 4, 11)]
    public void GetByReceiverAndAmountRangeShouldThrowExceptionIfNoReceiverWithAmountBetweenBoundriesFound(string reciver, decimal low, decimal hi)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => chainblock.GetByReceiverAndAmountRange(reciver, low, hi));

        Assert.AreEqual(Messages.NoReceiverWithSuchTransactionAmount, ex.Message);
    }

    [TestCase(8, 10.5)]
    [TestCase(7, 13)]
    public void GetAllInAmountRangeShouldReturnAllTransactionsWithinGivenRange(decimal low, decimal hi)
    {
        chainblock.Add(transaction);
        chainblock.Add(transactionTwo);

        Assert.AreEqual(new List<ITransaction> { transaction, transactionTwo }.Where(x => x.Amount >= low && x.Amount <= hi) as IEnumerable<ITransaction>, chainblock.GetAllInAmountRange(low, hi));
    }
}
