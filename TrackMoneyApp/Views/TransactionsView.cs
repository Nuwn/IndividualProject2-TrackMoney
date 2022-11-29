using System.Security.Principal;
using TrackMoney.TrackMoneyApp.Controllers;
using TrackMoney.TrackMoneyApp.Items;

namespace TrackMoney.TrackMoneyApp.Views
{
    public enum SortDirection { Ascending, Descending }
    public enum TransactionSorting { Amount, Title, Date }
    public enum Selection { Any, Expenses, Incomes }

    /// <summary>
    /// Setting for Transaction view, with default values
    /// </summary>
    public struct TransactionViewSettings
    {
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;
        public TransactionSorting TransactionSorting { get; set; } = TransactionSorting.Date;
        public Selection Selection { get; set; } = Selection.Any;
        public DateTime DateStart { get; set; } = DateTime.MinValue;
        public DateTime DateEnd { get; set; } = DateTime.MaxValue;

        public TransactionViewSettings(){}
    }


    public static class TransactionsView
    {
        /// <summary>
        /// Loads transitionViews with set setting or with default setting see <see cref="TransactionViewSettings"/>
        /// This method handles the initial showing of the view.
        /// </summary>
        /// <param name="transactionViewSettings"> Optional settings struct </param>

        public static void Load(TransactionViewSettings? transactionViewSettings = null)
        {
            Account account = Account.Get;
            TransactionViewSettings settings = transactionViewSettings ?? new();

            Console.Clear();

            ConsoleText.Get.Message("Transactions.").Show();
            ConsoleText.Get.Message($"Sorted by {settings.Selection}, {settings.TransactionSorting}({settings.SortDirection})").Show();
            Console.WriteLine();

            Console.WriteLine(string.Format("{0,-4}{1, -16}{2,-16}{3}", "ID", "Title", "Amount", "Date Added"));


            /**
             * Order sorts the query after transaction type <see cref="TransactionSorting/>
             */
            object Order(Transaction x) => settings.TransactionSorting switch
            {
                TransactionSorting.Date => x.Date,
                TransactionSorting.Title => x.Title,
                TransactionSorting.Amount => x.Amount,
                _ => x.Date,
            };

            /**
             * First selection data, we choose to get income, expense or all
             */
            bool Selector(Transaction s) => settings.Selection switch
            {
                Selection.Incomes => s.Amount > 0,
                Selection.Expenses => s.Amount < 0,
                Selection.Any => s.Amount != 0,
                _ => s.Amount != 0,
            };

            /**
             * Second selection data, we choose to get within a date range
             */
            bool DateRange(Transaction s) => s.Date >= settings.DateStart && s.Date <= settings.DateEnd;

            /**
             * Here we prepare the query, we need to use different methods for direction
             */
            var transactions = (settings.SortDirection == SortDirection.Ascending) ?
                account.Transactions.OrderBy(Order) :
                account.Transactions.OrderByDescending(Order);

            foreach (Transaction item in transactions.Where(Selector).Where(DateRange))
            {
                var title = item.Title.Length > 14 ? item.Title[..14] : item.Title;
                var id = account.Transactions.IndexOf(item);

                Console.WriteLine($"({id}) {title,-15} {item.Amount,-15} {item.Date:dd MMMM yyyy}");
            }


            TransactionViewMenu(settings, account);
        }

        /// <summary>
        /// The command part om the view, here we show menu and handle incomming commands.
        /// </summary>
        /// <param name="account">Which account we target</param>
        private static void TransactionViewMenu(TransactionViewSettings settings, Account account)
        {
            Console.WriteLine();
            ConsoleText.Get.ListedMessage(
                "(1) Show all transactions.",
                "(2) Show incomes only.",
                "(3) Show expenses only.",
                "(E) Edit transaction.",
                "(D) Delete Transaction.",
                "(N) New transaction.",
                "(S) Sort.",
                "(Y) Show Year",
                "(ESC) Return.").Show();

            InputController.AwaitKeyCommand(
                ((ConsoleKey command, Action callback))(ConsoleKey.D1, () =>
                { // Show all
                    Load(new()
                    {
                        Selection = Selection.Any,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.NumPad1, () => // Showall numpad
                {
                    Load(new()
                    {
                        Selection = Selection.Any,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.D2, () =>
                { // Show Incomes
                    Load(new()
                    {
                        Selection = Selection.Incomes,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                        DateStart = settings.DateStart,
                        DateEnd = settings.DateEnd,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.NumPad2, () => // Show Incomes numpad
                {
                    Load(new()
                    {
                        Selection = Selection.Incomes,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                        DateStart = settings.DateStart,
                        DateEnd = settings.DateEnd,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.D3, () =>
                { // Show Expenses
                    Load(new()
                    {
                        Selection = Selection.Expenses,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                        DateStart = settings.DateStart,
                        DateEnd = settings.DateEnd,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.NumPad3, () => // Show Expenses numpad
                {
                    Load(new()
                    {
                        Selection = Selection.Expenses,
                        TransactionSorting = settings.TransactionSorting,
                        SortDirection = settings.SortDirection,
                        DateStart = settings.DateStart,
                        DateEnd = settings.DateEnd,
                    });
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.E, () => // Edit transaction
                {
                    EditTransaction(account);
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.D, () => // Delete Transaction
                {
                    DeleteTransaction(account);
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.N, () => // New Transaction
                {
                    CreationView.Load();
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.S, () => // Sorting
                {
                    Sorted(settings);
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.Y, () => // Sorting
                {
                    ShowYear(settings);
                }),
                ((ConsoleKey command, Action callback))(ConsoleKey.Escape, () =>
                {
                    StartView.Load();
                })
            );
        }

        private static void EditTransaction(Account account)
        {
            ConsoleText.Get.Message("\nEnter id to edit:").Show();
            InputController.AwaitTextInput(
            (input) => CreationView.Load(int.Parse(input!)),
            // check if the number entered is within transactions count
            (input) => int.TryParse(input, out int value) && value >= 0 && value <= account.GetCount);
        }

        private static void DeleteTransaction(Account account)
        {
            ConsoleText.Get.Message("\nEnter id to delete:").Show();
            InputController.AwaitTextInput(
            (input) =>
            {
                int val = int.Parse(input!);
                account.DeleteTransaction(val);
                Load();
            },
            // check if the number entered is within transactions count
            (input) => int.TryParse(input, out int value) && value >= 0 && value <= account.GetCount);
        }

        /// <summary>
        /// Shows the sorted options in the menu
        /// </summary>
        /// <param name="settings"></param>
        private static void Sorted(TransactionViewSettings settings)
        {
            ConsoleText.Get.Message("\nSort:").Show();
            ConsoleText.Get.LinedMessage("(1) Title", "(2) Amount", "(3) Date").Show();

            TransactionSorting tSort = TransactionSorting.Date;
            InputController.AwaitKeyCommand(
                (ConsoleKey.D1, () =>
                {
                    tSort = TransactionSorting.Title;
                }),
                (ConsoleKey.D2, () =>
                {
                    tSort = TransactionSorting.Amount;
                }),
                (ConsoleKey.D3, () =>
                {
                    tSort = TransactionSorting.Date;
                }),
                (ConsoleKey.NumPad1, () =>
                {
                    tSort = TransactionSorting.Title;
                }),
                (ConsoleKey.NumPad2, () =>
                {
                    tSort = TransactionSorting.Amount;
                }),
                (ConsoleKey.NumPad3, () =>
                {
                    tSort = TransactionSorting.Date;
                })
            );

            ConsoleText.Get.LinedMessage("(1) Ascending", "(2) Descending").Show();

            SortDirection sortDir = SortDirection.Descending;
            InputController.AwaitKeyCommand(
                (ConsoleKey.D1, () =>
                {
                    sortDir = SortDirection.Ascending;
                }),
                (ConsoleKey.D2, () =>
                {
                    sortDir = SortDirection.Descending;
                }),
                (ConsoleKey.NumPad1, () =>
                {
                    sortDir = SortDirection.Ascending;
                }),
                (ConsoleKey.NumPad2, () =>
                {
                    sortDir = SortDirection.Descending;
                })
            );

            Load(new TransactionViewSettings()
            {
                Selection = settings.Selection,
                TransactionSorting = tSort,
                SortDirection = sortDir,
                DateStart = settings.DateStart,
                DateEnd = settings.DateEnd,
            });
        }

        /// <summary>
        /// Shows the sorted options in the menu
        /// </summary>
        /// <param name="settings"></param>
        private static void ShowYear(TransactionViewSettings settings)
        {
            ConsoleText.Get.Message("\nChoose year(xxxx):").Show();

            InputController.AwaitTextInput(
            (input) =>
            {
                int year = int.Parse(input!);
                DateTime start = DateTime.Parse($"01-01-{year}");
                DateTime end = DateTime.Parse($"12-31-{year}");

                Load(new()
                {
                    Selection = settings.Selection,
                    TransactionSorting = settings.TransactionSorting,
                    SortDirection = settings.SortDirection,
                    DateStart = start,
                    DateEnd = end
                });
            },
            (input) => int.TryParse(input, out int value) && value > 999 && value <= 9999);

        }
    }

}
