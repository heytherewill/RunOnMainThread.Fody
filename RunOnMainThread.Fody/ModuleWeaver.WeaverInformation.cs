using System.Linq;
using Mono.Cecil;
using static WeaverInfo;

public partial class ModuleWeaver
{
    private WeaverInfo WeaverInformation(MethodDefinition method)
    {
        if (method.ReturnType == _void)
            return Void(method);

        var sequencePoint = method.DebugInformation.SequencePoints.FirstOrDefault();
        return None(sequencePoint);
    }
}
