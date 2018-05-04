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
        Debug.WriteLine("Weaving file: " + ModuleDefinition.FileName);

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
                method.CustomAttributes.Remove(method.CustomAttributes
                    .First(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName));
            }
        }
    }

    private void Initialize()
    {
        //Assemblies
        _runOnMainThreadAssembly = FindAssembly(RunOnMainThreadAssemblyName);

        //Types
        _void = ModuleDefinition.TypeSystem.Void;
        _bool = ModuleDefinition.TypeSystem.Boolean;

        _mainThreadDispatcher = new TypeReference(
            RunOnMainthreadNamespaceName, MainThreadDispatcherTypeName,
            ModuleDefinition, _runOnMainThreadAssembly);

        _action = new TypeReference(
            SystemAssemblyName,
            ActionTypeName,
            ModuleDefinition, ModuleDefinition.TypeSystem.CoreLibrary);

        _mainThreadAttribute = new TypeReference(
            RunOnMainThreadAssemblyName,
            RunOnMainThreadAttributeTypeName,
            ModuleDefinition, _runOnMainThreadAssembly);

        //Methods
        _actionConstructor = ModuleDefinition.ImportReference(typeof(Action).GetConstructors().Single());

        _runOnMainThread = new MethodReference(RunOnMainThreadName, _void, _mainThreadDispatcher);
        _runOnMainThread.Parameters.Add(new ParameterDefinition(_action));
    }

    private AssemblyNameReference FindAssembly(string assemblyName)
        => ModuleDefinition.AssemblyReferences.SingleOrDefault(a => a.Name == assemblyName);

    private IEnumerable<MethodDefinition> MethodsToWeave(TypeDefinition type)
        => type.Methods.Where(MethodHasMainThreadAttribute);

    private bool MethodHasMainThreadAttribute(MethodDefinition method)
        => method.CustomAttributes.Any(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName);
}