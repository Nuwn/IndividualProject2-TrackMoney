using TrackMoney.TrackMoneyApp.Controllers;
using TrackMoney.TrackMoneyApp.Items;

namespace TrackMoney.TrackMoneyApp.Views
{
    public static class CreationView
    {
        public static void Load(int? replaceIndex = null)
        {
            bool replacing = replaceIndex != null;

            Account account = Account.Get;

            Console.Clear();
            ConsoleText.Get.Message(replacing ? "Editing transaction:" : "Creating Transaction:").Show();

            /**
             * 
             * First we ask if the user wants to add an income or expense 
             * 
             */
            ConsoleText.Get.Message("Select Transaction type:").Show();
            ConsoleText.Get.ListedMessage("(1) Income","(2) Expense").Show();

            bool income = true;

            InputController.AwaitKeyCommand(
                (ConsoleKey.D1, () => 
                { 
                    income = true;
                }), 
                (ConsoleKey.D2, () => 
                { 
                    income = false;
                })
            );


            /**
             * 
             * Here we collect the new title or if they press enter, we select old one.  
             * 
             * */
            var title = replacing ? account.Transactions[replaceIndex!.Value].Title : "";
            ConsoleText.Get.Message($"Enter new Title({title}):").Show();

            InputController.AwaitTextInput((str) => {
                title = str ?? title;
            });

            /**
             * 
             * Here we collect the amount, in positive value, we also validate that it is positive and a number,
             * If they want old value they have to type it in.
             * 
             * */
            decimal amount = replacing ? account.Transactions[replaceIndex!.Value].Amount : 0;
            ConsoleText.Get.Message($"Enter amount({Math.Abs(amount)}):").Show();

            InputController.AwaitTextInput((input) => 
            {
                amount = decimal.Parse(input!);
            }, (input) =>
            {
                // check that it's a decimal and a positive number
                return decimal.TryParse(input, out decimal result) && result > 0;
                // add more validation if needed, like max amount
            });

            /**
             * 
             * Here we select the date yyyy-MM-dd. and not just month.
             * 
             * */
            DateTime date = replacing ? account.Transactions[replaceIndex!.Value].Date : DateTime.MinValue;
            ConsoleText.Get.Message($"Enter date({date}):").Show();

            InputController.AwaitTextInput((input) =>
            {
                date = DateTime.Parse(input!);
            }, (input) =>
            {
                return DateTime.TryParse(input, out DateTime result);
            });


            /**
             * 
             * Last part we Update the transaction or add a new one.
             * 
             * */
            Transaction newTransaction = new(title, income ? amount : amount * -1, date);

            if (replacing)
            {
                account.Transactions[replaceIndex!.Value] = newTransaction;
                ConsoleText.Get.Message("Transaction updated...").Show();
            }
            else
            {
                account.AddTransaction(newTransaction);
                ConsoleText.Get.Message("Transaction added...").Show();
            }

            ConsoleText.Get.Message("Press any key to continue...").Show();


            InputController.AwaitKeyInput((key) =>
            {
                TransactionsView.Load();
            });
        }
    }
}
