using EventsToAwaitables;
using EventsToAwaitables.GenericExtensionMethodWithDelegates;
using EventsToAwaitables.GenericExtensionMethodWithReflection;
using EventsToAwaitables.GenericExtensionMethodWithReflectionAndCustomAwaitable;
using EventsToAwaitables.SimpleExtensionMethod;

var sc = new CustomSynchronizationContext();
sc.Post(_ => ApplicationMain(), null);
async void ApplicationMain()
{
    Console.WriteLine("> Normal event");
    NormalEventDemo.Run();
    await Task.Delay(100);
    Console.WriteLine("--------------\n");

    Console.WriteLine("> Awaitable event with ad hoc extension method");
    await AwaitableEventWithSimpleExtensionMethod.Run();
    await Task.Delay(100);
    Console.WriteLine("--------------\n");

    Console.WriteLine("> Awaitable event with subscribe and unsubscribe delegates");
    await GenericExtensionMethodWithDelegates.Run();
    await Task.Delay(100);
    Console.WriteLine("--------------\n");

    Console.WriteLine("> Awaitable event with reflection");
    await GenericExtensionMethodWithReflection.Run();
    await Task.Delay(100);
    Console.WriteLine("--------------\n");

    Console.WriteLine("> Awaitable event with reflection plus a custom awaitable that always run the continuation synchronously");
    await GenericExtensionMethodWithReflectionAndCustomAwaitable.Run();
    await Task.Delay(100);
    Console.WriteLine("--------------\n");
}

Console.Read();
