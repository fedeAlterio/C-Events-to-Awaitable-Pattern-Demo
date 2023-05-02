namespace EventsToAwaitables.GenericExtensionMethodWithReflection
{
    public static class GenericExtensionMethodWithReflection
    {
        public static async Task Run()
        {
            var game = new Game();
            StartGameAfter1Second(game);
            await game.WaitEvent(nameof(Game.Started));
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

    public static class AsyncEx
    {
        public static async Task WaitEvent<T>(this T @this, string eventName) where T : notnull
        {
            var eventInfo = @this.GetType().GetEvent(eventName);
            var tcs = new TaskCompletionSource();
            Delegate handler = SetResult;
            eventInfo!.AddEventHandler(@this, handler);

            void SetResult() => tcs.SetResult();
            try
            {
                await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                eventInfo!.RemoveEventHandler(@this, handler);
            }
        }
    }
}
