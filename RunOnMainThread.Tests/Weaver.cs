using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyToWeave;
using Mono.Cecil;
using RunOnMainThread;
using Xunit;

public class WeaverTests
{
    private const string AssemblyToWeaveDll = "AssemblyToWeave.dll";

    private readonly Assembly wovenAssembly;

    public WeaverTests()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyPath = 
            Path.GetFullPath(Path.Combine(assemblyLocation, $@"../../../../../TestAssemblies/Debug/netstandard2.0/{AssemblyToWeaveDll}"))
#if (!DEBUG)
            .Replace("Debug", "Release")
#endif
        ;

        var wovenAssemblyPath = assemblyPath.Replace(".dll", "_Woven.dll");
        File.Copy(assemblyPath, wovenAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        var weavingTask = new ModuleWeaver { ModuleDefinition = moduleDefinition };

        weavingTask.Execute();
        moduleDefinition.Write(wovenAssemblyPath);

        wovenAssembly = Assembly.LoadFile(wovenAssemblyPath);
    }

    [Fact]
    public void MethodsAnnotedWithTheRunOnMainThreadAttributeCallTheMainThreadDispatcher()
    {
        var type = wovenAssembly.DefinedTypes.Single(t => t.Name == nameof(BeforeWeave));
        var instance = (dynamic)Activator.CreateInstance(type);
        instance.ThisRunsOnTheMainThread();

        Assert.Equal(1, MainThreadDispatcher.Count);
    }
}
