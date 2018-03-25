using System;

namespace MainThread
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RunOnMainThreadAttribute : Attribute
    {
    }
}
