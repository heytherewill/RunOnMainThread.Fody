using RunOnMainThread;

namespace RunOnMainThreadSample.Core
{
    public sealed class DialogController
    {
        private readonly IDialogDisplayer _displayer;

        public DialogController(IDialogDisplayer displayer)
        {
            _displayer = displayer;
        }

        public void ShowDialog()
        {
            _displayer.ShowDialog("This crashes");
        }

        public void ShowDialogUsingDispatcher()
        {
            MainThreadDispatcher.RunOnMainThread(() =>
            {
                _displayer.ShowDialog("This runs on the main thread because of the dispatcher");
            });
        }

        [RunOnMainThread]
        public void ShowDialogUsingWeaver()
        {
            _displayer.ShowDialog("This runs on the main thread because of the weaver");
        }
    }
}
