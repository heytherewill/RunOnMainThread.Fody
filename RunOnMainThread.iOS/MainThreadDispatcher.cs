using System;
using Foundation;

namespace RunOnMainThread
{
    namespace RunOnMainThread
    {
        public static class MainThreadDispatcher
        {
            public static void RunOnMainThread(Action action)
            {
                NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
            }
        }
    }
}
