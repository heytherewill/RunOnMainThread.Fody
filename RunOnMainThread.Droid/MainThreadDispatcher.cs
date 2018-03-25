using System;
using Android.OS;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        private static Handler _handler;

        public static void RunOnMainThread(Action action)
        {
            if (_handler == null || _handler.Looper != Looper.MainLooper)
            {
                _handler = new Handler(Looper.MainLooper);
            }

            _handler.Post(action);
        }
    }
}
