using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    // Will log an MessageImportance.High message to MSBuild. OPTIONAL
    public Action<string> LogInfo { get; set; }

    // Will log an warning message to MSBuild. OPTIONAL
    public Action<string> LogWarning { get; set; }

    // Will log an error message to MSBuild. OPTIONAL
    public Action<string> LogError { get; set; }

    // Will log an warning message to MSBuild at a specific point in the code. OPTIONAL
    public Action<string, SequencePoint> LogWarningPoint { get; set; }

    // Will log an error message to MSBuild at a specific point in the code. OPTIONAL
    public Action<string, SequencePoint> LogErrorPoint { get; set; }

    private const string ReturnTypeMustBeVoid = "You can only use the RunOnMainThreadAttribute on void methods";

    private const string ActionTypeName = "Action";
    private const string SystemAssemblyName = "System";
    private const string ConstructorMethodName = ".ctor";
    private const string RunOnMainThreadName = "RunOnMainThread";
    private const string RunOnMainThreadAssemblyName = "RunOnMainThread";
    private const string RunOnMainthreadNamespaceName = "RunOnMainThread";
    private const string MainThreadDispatcherTypeName = "MainThreadDispatcher";
    private const string RunOnMainThreadAttributeTypeName = "RunOnMainThreadAttribute";

    private static AssemblyNameReference RunOnMainThreadAssembly;

    private TypeReference Bool;
    private TypeReference Void;
    private TypeReference Action;
    private TypeReference MainThreadAttribute;
    private TypeReference MainThreadDispatcher;

    private MethodReference RunOnMainThread;
    private MethodReference ActionConstructor;
}