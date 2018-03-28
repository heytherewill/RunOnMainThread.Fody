using System;
using System.Windows;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        public static void RunOnMainThread(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
