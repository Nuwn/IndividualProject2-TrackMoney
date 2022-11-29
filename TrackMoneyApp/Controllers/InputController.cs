using System;
using TrackMoney.TrackMoneyApp.Items;
using TrackMoney.TrackMoneyApp.Views;

namespace TrackMoney.TrackMoneyApp.Controllers
{
    /// <summary>
    /// This controller handles all inputs, and waits until the right input is made, with while loop and validation.
    /// </summary>
    public static class InputController
    {
        public static void AwaitTextCommand(params (string command, Action callback)[] commands)
        {
            string? input = Console.ReadLine();
            commands.FirstOrDefault((x) => x.command.ToLower() == input?.ToLower()).callback?.Invoke();
        }

        public static void AwaitKeyCommand(params (ConsoleKey command, Action callback)[] commands)
        {
            ConsoleKeyInfo? input;
            do
            {
                input = Console.ReadKey(true);
            } while (!commands.Any(x => x.command == input.Value.Key));

            commands.First((x) => x.command == input.Value.Key).callback?.Invoke();
        }
        /// <summary>
        /// Waits for a single key input, then sends in to the callback.
        /// </summary>
        /// <param name="Callback">Final callback. returns the key input</param>
        /// <param name="predicate">Validation func, call back is called when this returns true</param>
        public static void AwaitKeyInput(Action<ConsoleKey> Callback, Func<ConsoleKey, bool>? predicate = null)
        {
            ConsoleKey key;

            do
                key = Console.ReadKey().Key;  
            while (predicate != null && !predicate(key));

            Callback?.Invoke(key);
        }

        public static void AwaitTextInput(Action<string?> Callback, Func<string, bool>? predicate = null)
        {
            string? str;

            do
                str = Console.ReadLine();
            while (predicate != null && !predicate(str ?? string.Empty));

            Callback?.Invoke(str);
        }

        internal static void AwaitKeyCommand(object value1, object value2, object value3, object value4)
        {
            throw new NotImplementedException();
        }
    }

}