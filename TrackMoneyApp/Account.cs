using SaveSystem;
using TrackMoney.TrackMoneyApp.Items;

namespace TrackMoney.TrackMoneyApp
{
    /// <summary>
    /// the account class is made to handle account type of actions, Holding the transactions, actions between "storage"(save),
    /// Decided to make it using the singleton strategy so that we only have 1 account active.
    /// </summary>
    
    public sealed class Account
    {
        public List<Transaction> Transactions { get; private set; }

        private static Account? singleton;
        public static Account Get => singleton ??= new Account();

        public Account()
        {
            Transactions = SaveManager.Load<List<Transaction>>("Account") ?? new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            Save();
        }

        public void DeleteTransaction(int index)
        {
            Transactions.RemoveAt(index);
            Save();
        }

        public void UpdateTransaction(Transaction transaction, Transaction newTransaction)
        {
            Transactions.Remove(transaction);
            Transactions.Add(newTransaction);
            Save();
        }

        private void Save() => SaveManager.Save("Account", Transactions);

        public decimal? GetTotal => Transactions.Sum(x => x.Amount);
        public int GetCount => Transactions.Count;
    }
}
