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
        /// <summary>
        /// Simply awaits for a text input and then calls the watching callback
        /// e.g: user writes "blue" and we have registeded blue as a command, it will be called.
        /// </summary>
        public static void AwaitTextCommand(params (string command, Action callback)[] commands)
        {
            string? input = Console.ReadLine();
            commands.FirstOrDefault((x) => x.command.ToLower() == input?.ToLower()).callback?.Invoke();
        }

        /// <summary>
        /// Awaits a single key, then matches it to registred commands, if found, call the callback.
        /// </summary>
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

        /// <summary>
        /// Awaits a single text input, e.g you ask user to type a search query. 
        /// </summary>
        /// <param name="Callback">Callback that returns the string typed</param>
        /// <param name="predicate">Validator, if true, callback will be called.</param>
        public static void AwaitTextInput(Action<string?> Callback, Func<string, bool>? predicate = null)
        {
            string? str;

            do
                str = Console.ReadLine();
            while (predicate != null && !predicate(str ?? string.Empty));

            Callback?.Invoke(str);
        }
    }

}