using Mono.Cecil;
using Mono.Cecil.Cil;

public enum WeaverKind
{
    None,
    Void,
}

public struct WeaverInfo
{
    public WeaverKind WeaverKind { get; }

    public SequencePoint SequencePoint { get; }

    public MethodDefinition MethodDefinition { get; }

    private WeaverInfo(WeaverKind weaverKind, MethodDefinition methodDefinition, SequencePoint sequencePoint)
    {
        WeaverKind = weaverKind;
        SequencePoint = sequencePoint;
        MethodDefinition = methodDefinition;
    }

    public static WeaverInfo None(SequencePoint sequencePoint)
        => new WeaverInfo(WeaverKind.None, null, sequencePoint);

    public static WeaverInfo Void(MethodDefinition methodDefinition)
        => new WeaverInfo(WeaverKind.Void, methodDefinition, null);
}
