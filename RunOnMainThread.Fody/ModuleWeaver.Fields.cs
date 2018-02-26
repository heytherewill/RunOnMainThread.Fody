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

    private const string ReturnMustBeVoid = "You can only use the RunOnMainThreadAttribute on void methods";

    private const string ActionTypeName = "Action";
    private const string SystemAssemblyName = "System";
    private const string ConstructorMethodName = ".ctor";
    private const string RunOnMainThreadName = "RunOnMainThread";
    private const string RunOnMainThreadAssemblyName = "RunOnMainThread";
    private const string RunOnMainthreadNamespaceName = "RunOnMainThread";
    private const string MainThreadDispatcherTypeName = "MainThreadDispatcher";
    private const string RunOnMainThreadAttributeTypeName = "RunOnMainThreadAttribute";

    private static AssemblyNameReference _runOnMainThreadAssembly;

    private TypeReference _bool;
    private TypeReference _void;
    private TypeReference _action;
    private TypeReference _mainThreadAttribute;
    private TypeReference _mainThreadDispatcher;

    private MethodReference _runOnMainThread;
    private MethodReference _actionConstructor;
}