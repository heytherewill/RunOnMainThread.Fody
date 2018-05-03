using System;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        public static void RunOnMainThread(Action action)
            => throw new PlatformNotSupportedException("No platform specific MainThreadDispatcher has been provided for this platform");
    }
}
