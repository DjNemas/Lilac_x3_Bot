namespace Lilac_x3_Bot
{
    class Program
    {
        public static void Main(string[] args)
            => new InitBot().MainAsync().GetAwaiter().GetResult();
    }
}
