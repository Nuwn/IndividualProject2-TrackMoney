using TrackMoney.TrackMoneyApp.Controllers;
using TrackMoney.TrackMoneyApp.Items;

namespace TrackMoney.TrackMoneyApp.Views
{
    public static class StartView
    {
        public static void Load()
        {
            Account account = Account.Get;
            Console.Clear();

            ConsoleText.Get.Message("Welcome to trackMoney!").Show();
            ConsoleText.Get.Message($"You currently have {account.GetTotal}:- on your  account.").Show();
            Console.WriteLine();
            ConsoleText.Get.Message("Pick an option:").Show();

            ConsoleText.Get
                .ListedMessage("(C) List all Incomes and Expenses.", "(N) Add new Income/Expense", "(ESC) Exit.")
                .Show();

            InputController.AwaitKeyCommand( 
                (ConsoleKey.C, new Action(() => {
                    TransactionsView.Load();
                })),
                (ConsoleKey.N, new Action(() => {
                    CreationView.Load();
                })),
                (ConsoleKey.Escape, new Action(() => {}))
            );
        }
    }
}
