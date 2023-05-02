namespace EventsToAwaitables
{
    public class Game
    {
        public event Action? Started;

        public void Start() 
        {
            Started?.Invoke();
            Console.WriteLine("Game started notified");
        }
    }
}
