namespace EventsToAwaitables.GenericExtensionMethodWithDelegates
{
    public static class GenericExtensionMethodWithDelegates
    {
        public static async Task Run()
        {
            var game = new Game();
            StartGameAfter1Second(game);
            await AsyncEx.WaitEvent(handler => game.Started += handler, handler => game.Started -= handler);
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
        public static async Task WaitEvent(Action<Action> subscriber,
                                           Action<Action> unsubscriber)
        {
            var tcs = new TaskCompletionSource();

            subscriber.Invoke(SetResult);

            void SetResult() => tcs.SetResult();
            try
            {
                await tcs.Task;
            }
            finally
            {
                unsubscriber.Invoke(SetResult);
            }
        }
    }
}
