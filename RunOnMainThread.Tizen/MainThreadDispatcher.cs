using System;
using System.Threading;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        public static void RunOnMainThread(Action action)
        {
            SynchronizationContext.Current.Post(o => action(), null);
        }
    }
}
