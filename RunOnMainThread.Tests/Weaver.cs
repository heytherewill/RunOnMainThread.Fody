using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

public class WeaverTests
{
    private Assembly wovenAssembly;

    public void Setup()
    {
        var assemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../../MvxMainThread.AssemblyToProcess/bin/Debug/MvxMainThread.AssemblyToProcess.dll"));
#if (!DEBUG)
        assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

        var wovenAssemblyPath = assemblyPath.Replace(".dll", "Woven.dll");
        File.Copy(assemblyPath, wovenAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        var weavingTask = new ModuleWeaver { ModuleDefinition = moduleDefinition };

        weavingTask.Execute();
        moduleDefinition.Write(wovenAssemblyPath);

        wovenAssembly = Assembly.LoadFile(wovenAssemblyPath);

        InitializeSingletonsIfNeeded();
    }

    [Fact]
    public void MethodsAnnotedWithTheRunOnMainThreadAttributeCallTheMainThreadDispatcher()
    {
        var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>() as CountingDispatcher;
        var calls = dispatcher.Calls;

        var instance = (dynamic)Activator.CreateInstance(wovenAssembly.GetType(nameof(ToBeWoven)));
        instance.SomeWovenMainThreadMethod();

        Assert.Greater(dispatcher.Calls, calls);
    }
}
