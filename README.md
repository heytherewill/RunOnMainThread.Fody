# RunOnMainThread.Fody
Run annotated methods on the Main Thread.
The Fody plugin you never realized you needed. 

# Installation

Install via [NuGet](https://www.nuget.org/packages/RunOnMainThread.Fody/) using:

`PM> Install-Package RunOnMainThread.Fody`

Then add it to your `FodyWeavers.xml`:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Weavers>
  <RunOnMainThread />
</Weavers>
```

# Usage

The package will add a static `MainThreadDispatcher` class with a `void RunOnMainThread(Action action)` method. 
You can use it directly to access the main thread from anywhere in your app:

```csharp
private void ShowDialogUsingDispatcher()
{
    MainThreadDispatcher.RunOnMainThread(() =>
    {
        Console.WriteLine("Main thread!");
    });
}
```

The cool part about this library, though, is that you can instead annotate your method with the `RunOnMainThreadAttribute` and all that boilerplate will be added for you.
The method below is the same as the one in the static class example:

```csharp
[RunOnMainThread]
private void ShowDialogUsingWeaver()
{
    Console.WriteLine("Main thread!");
}
```

For more examples, check out the sample apps.
