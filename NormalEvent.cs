using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsToAwaitables
{
    public static class NormalEventDemo
    {
        public static void Run()
        {            
            // Used to be sure that the Start method is called at the return of this method
            ManualResetEvent handle = new ManualResetEvent(false);

            var game = new Game();
            StartGameAfter1Second(game);
            game.Started += () =>
            {
                Console.WriteLine("Started");
                Console.WriteLine($"Handler thread {Thread.CurrentThread.ManagedThreadId}");
                handle.Set();
            };

            handle.WaitOne();
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
}
