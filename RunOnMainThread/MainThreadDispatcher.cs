using System;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        public static int Count = 0;

        public static void RunOnMainThread(Action action)
        {
            Count++;
            action();
        }
    }
}
