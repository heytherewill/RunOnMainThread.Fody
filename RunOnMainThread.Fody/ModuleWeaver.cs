using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public ModuleDefinition ModuleDefinition { get; set; }

    public void Execute()
    {
        Initialize();

        var typesToWeave =
            ModuleDefinition
                .GetTypes()
                .SelectMany(MethodsToWeave)
                .GroupBy(method => method.DeclaringType);

        foreach (var grouping in typesToWeave)
        {
            var declaringType = grouping.Key;

            foreach (var weaverInfo in grouping.Select(WeaverInformation))
            {
                switch (weaverInfo.WeaverKind)
                {
                    case WeaverKind.Void:
                        WeaveInvokeOnMainThreadOnVoidMethod(weaverInfo.MethodDefinition, declaringType);
                        break;

                    case WeaverKind.None:
                        LogErrorPoint(ReturnTypeMustBeVoid, weaverInfo.SequencePoint);
                        break;
                }

                //Removes the reference to the RunOnMainThread attribute
                weaverInfo.MethodDefinition.CustomAttributes.Remove(weaverInfo.MethodDefinition.CustomAttributes
                    .First(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName));
            }
        }
    }

    private void Initialize()
    {
        //Assemblies
        RunOnMainThreadAssembly = FindAssembly(RunOnMainThreadAssemblyName);

        //Types
        Void = ModuleDefinition.TypeSystem.Void;
        Bool = ModuleDefinition.TypeSystem.Boolean;

        MainThreadDispatcher = new TypeReference(
            RunOnMainthreadNamespaceName, MainThreadDispatcherTypeName,
            ModuleDefinition, RunOnMainThreadAssembly);

        Action = new TypeReference(
            SystemAssemblyName,
            ActionTypeName,
            ModuleDefinition, ModuleDefinition.TypeSystem.CoreLibrary);

        MainThreadAttribute = new TypeReference(
            RunOnMainThreadAssemblyName,
            RunOnMainThreadAttributeTypeName,
            ModuleDefinition, RunOnMainThreadAssembly);

        //Methods
        ActionConstructor = ModuleDefinition.ImportReference(typeof(Action).GetConstructors().Single());

        RunOnMainThread = new MethodReference(RunOnMainThreadName, Void, MainThreadDispatcher);
        RunOnMainThread.Parameters.Add(new ParameterDefinition(Action));
    }

    private AssemblyNameReference FindAssembly(string assemblyName)
        => ModuleDefinition.AssemblyReferences.SingleOrDefault(a => a.Name == assemblyName);

    private IEnumerable<MethodDefinition> MethodsToWeave(TypeDefinition type)
        => type.Methods.Where(MethodHasMainThreadAttribute);

    private bool MethodHasMainThreadAttribute(MethodDefinition method)
        => method.CustomAttributes.Any(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName);
}