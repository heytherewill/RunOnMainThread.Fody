using System;
using RunOnMainThread;

namespace AssemblyToWeave
{
    public sealed class AfterWeave
    {
        public void ThisRunsOnTheMainThread()
        {
            MainThreadDispatcher.RunOnMainThread(__ThisRunsOnTheMainThread_Woven);
        }

        private void __ThisRunsOnTheMainThread_Woven()
        {
            Console.WriteLine("Blah");
        }
    }
}
