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
}
