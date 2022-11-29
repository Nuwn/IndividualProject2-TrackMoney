namespace TrackMoney.TrackMoneyApp.Items
{
    /// <summary>
    /// A Simple class to format the text output better.
    /// </summary>
    public sealed class ConsoleText
    {
        private static ConsoleText? singleton;
        public static ConsoleText Get => singleton ??= new ConsoleText();


        private string message = string.Empty;
        private ConsoleColor color = ConsoleColor.White;

        public ConsoleText Message(string v)
        {
            message = v;
            return this;
        }

        public ConsoleText LinedMessage(params string[] text)
        {
            message = OutputLine(text);
            return this;
        }
        public ConsoleText ListedMessage(params string[] text)
        {
            message = OutputList(text);
            return this;
        }

        public ConsoleText Color(ConsoleColor color)
        {
            this.color = color;
            return this;
        }

        public void Show()
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.WriteLine(message);

            Console.ForegroundColor = old;
        }

        static string OutputLine(params string[] text) => string.Join(';', text).Replace(";", " | ");
        static string OutputList(params string[] text) => string.Join('\n', text);
    }
}