using System;
using RunOnMainThread;

namespace AssemblyToWeave
{
    public sealed class BeforeWeave
    {
        [RunOnMainThread]
        public void ThisRunsOnTheMainThread()
        {
            Console.WriteLine("Blah");
        }
    }

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
