namespace SnakeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.InitGame();
            game.Start();
        }
    }
}
