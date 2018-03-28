using System;

namespace RunOnMainThread
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RunOnMainThreadAttribute : Attribute
    {
    }
}
