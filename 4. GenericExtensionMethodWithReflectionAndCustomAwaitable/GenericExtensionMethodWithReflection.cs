using System.Runtime.CompilerServices;

namespace EventsToAwaitables.GenericExtensionMethodWithReflectionAndCustomAwaitable
{
    public static class GenericExtensionMethodWithReflectionAndCustomAwaitable
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
        public static Awaitable WaitEvent<T>(this T @this, string eventName) where T : notnull
        {
            var eventInfo = @this.GetType().GetEvent(eventName);
            var awaitable = new Awaitable();
            Delegate handler = null!;
            handler = SetResult;
            eventInfo!.AddEventHandler(@this, handler);
            void SetResult() => awaitable.ContinueWith(() =>
                                eventInfo!.RemoveEventHandler(@this, handler));
            return awaitable;
        }
    }

    public class Awaitable : INotifyCompletion
    {
        Action? _continuation;
        Action? _continueWith;
        public Awaitable GetAwaiter() => this;
        public bool IsCompleted => false;
        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
            ExecuteContinuationIfPossible();
        }

        public void GetResult() { }
        public void ContinueWith(Action continueWith)
        {
            _continueWith = continueWith;
            ExecuteContinuationIfPossible();
        }

        void ExecuteContinuationIfPossible()
        {
            if (_continuation is null || _continueWith is null)
                return;

            _continuation.Invoke();
            _continueWith();
        }

    }
}
