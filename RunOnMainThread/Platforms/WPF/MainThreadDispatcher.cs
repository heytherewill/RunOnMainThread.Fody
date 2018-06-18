using System;
using System.Windows;

namespace RunOnMainThread.Platforms.WPF
{
    public static class MainThreadDispatcher
    {
        public static void RunOnMainThread(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
