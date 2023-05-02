namespace EventsToAwaitables.SimpleExtensionMethod
{
    public static class AwaitableEventWithSimpleExtensionMethod
    {
        public static async Task Run()
        {
            var game = new Game();
            StartGameAfter1Second(game);
            await game.StartedAsync();
            Console.WriteLine("Started");
            Console.WriteLine($"Handler thread {Thread.CurrentThread.ManagedThreadId}");
        }

        static void StartGameAfter1Second(Game game)
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Event raiser thread {Thread.CurrentThread.ManagedThreadId}");
                game.Start();
            }).Start();
        }
    }

    public static class GameExtensions
    {
        public static async Task StartedAsync(this Game game)
        {
            var tcs = new TaskCompletionSource();
            game.Started += Game_Started;

            void Game_Started() => tcs.SetResult();
            try
            {
                await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                game.Started -= Game_Started;
            }
        }
    }
}
