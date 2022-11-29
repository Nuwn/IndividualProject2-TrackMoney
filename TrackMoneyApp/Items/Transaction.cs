namespace TrackMoney.TrackMoneyApp.Items
{
    [Serializable]
    public struct Transaction
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public Transaction(string title, decimal amount, DateTime? date = null)
        {
            Title = title;
            Amount = amount;
            Date = date ?? DateTime.Now;
        }
    }
}
