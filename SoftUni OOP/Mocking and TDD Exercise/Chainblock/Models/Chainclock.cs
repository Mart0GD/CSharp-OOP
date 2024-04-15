using Chainblock.Contracts;
using Chainblock.Enums;
using Chainblock.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chainblock.Models
{
    public class Chainclock : IChainblock
    {
        public int Count => transactions.Count;

        IDictionary<int, ITransaction> transactions;

        public Chainclock()
        {
            transactions = new Dictionary<int, ITransaction>();
        }

        public void Add(ITransaction tx)
        {
            if (Contains(tx))
            {
                throw new InvalidOperationException(Messages.DuplicateId);
            }

            transactions.Add(tx.Id, tx);
        }

        public bool Contains(int id) => transactions.ContainsKey(id);

        public bool Contains(ITransaction transaction) => transactions.ContainsKey(transaction.Id);

        public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
        {
            if (!Contains(id))
            {
                throw new InvalidOperationException(Messages.IncorrectId);
            }

            transactions[id].Status = newStatus;
        }

        public void RemoveTransactionById(int id)
        {
            if (!Contains(id))
            {
                throw new InvalidOperationException(Messages.IncorrectId);
            }

            transactions.Remove(id);
        }

        public ITransaction GetById(int id)
        {
            if (!Contains(id))
            {
                throw new InvalidOperationException(Messages.IncorrectId);
            }

            return transactions[id];
        }

        public IEnumerable<ITransaction> GetByTransactionStatus(TransactionStatus status)
        {
            if (!transactions.Any(x => x.Value.Status == status))
            {
                throw new InvalidOperationException(Messages.IncorrectStatus);
            }

            return transactions.Where(x => x.Value.Status == status).OrderByDescending(x => x.Value.Amount).Select(x => x.Value);
        }

        public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
        {
            if (!transactions.Any(x => x.Value.Status == status))
            {
                throw new InvalidOperationException(Messages.IncorrectStatus);
            }

            return transactions.Where(x => x.Value.Status == status).OrderByDescending(x => x.Value.Amount).Select(x => x.Value.From);
        }

        public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
        {
            if (!transactions.Any(x => x.Value.Status == status))
            {
                throw new InvalidOperationException(Messages.IncorrectStatus);
            }

            return transactions.Where(x => x.Value.Status == status).OrderByDescending(x => x.Value.Amount).Select(x => x.Value.To);
        }

        public IEnumerable<ITransaction> GetAllOrderedByAmountDescendingThenById() => transactions.OrderByDescending(x => x.Value.Amount).ThenBy(x => x.Value.Id).Select(x => x.Value);

        public IEnumerable<ITransaction> GetBySenderOrderedByAmountDescending(string sender)
        {
            if (!transactions.Any(x => x.Value.From == sender))
            {
                throw new InvalidOperationException(Messages.IncorrectSender);
            }

            return transactions.Where(x => x.Value.From == sender).OrderByDescending(x => x.Value.Amount).Select(x => x.Value);
        }

        public IEnumerable<ITransaction> GetByReceiverOrderedByAmountThenById(string receiver)
        {

            if (!transactions.Any(x => x.Value.To == receiver))
            {
                throw new InvalidOperationException(Messages.IncorrectReceiver);
            }

            return transactions.Where(x => x.Value.To == receiver).OrderByDescending(x => x.Value.Amount).ThenBy(x => x.Value.Id).Select(x => x.Value);
        }

        public IEnumerable<ITransaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, decimal amount) => transactions.Select(x => x.Value).Where(x => x.Amount <= amount && x.Status == status).OrderByDescending(x => x.Amount);

        public IEnumerable<ITransaction> GetBySenderAndMinimumAmountDescending(string sender, decimal amount)
        {
            if (!transactions.Any(x => x.Value.From == sender && x.Value.Amount > amount))
            {
                throw new InvalidOperationException(Messages.NoSenderWithSuchTransactionAmount);
            }

            return transactions.Where(x => x.Value.From == sender && x.Value.Amount > amount).OrderByDescending(x => x.Value.Amount).Select(x => x.Value);
        }

        public IEnumerable<ITransaction> GetByReceiverAndAmountRange(string receiver, decimal lo, decimal hi)
        {
            if (!transactions.Any(x => x.Value.To == receiver && x.Value.Amount >= lo && x.Value.Amount < hi))
            {
                throw new InvalidOperationException(Messages.NoReceiverWithSuchTransactionAmount);
            }

            return transactions.Where(x => x.Value.To == receiver && x.Value.Amount >= lo && x.Value.Amount < hi).OrderByDescending(x => x.Value.Amount).ThenBy(x => x.Value.Id).Select(x => x.Value);
        }

        public IEnumerable<ITransaction> GetAllInAmountRange(decimal lo, decimal hi) => transactions.Select(x => x.Value).Where(x => x.Amount >= lo && x.Amount <= hi);
    }
}
