using System;
using System.Threading;
using RunOnMainThreadSample.Core;
using UIKit;

namespace RunOnMainThreadSample.iOS
{
    public partial class ViewController : UIViewController, IDialogDisplayer
    {
        private DialogController _dialogController;

        protected ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _dialogController = new DialogController(this);

            CrashButton.TouchUpInside += delegate
            {
                var thread = new Thread(_dialogController.ShowDialog);
                thread.Start();
            };

            DispatcherButton.TouchUpInside += delegate
            {
                var thread = new Thread(_dialogController.ShowDialogUsingDispatcher);
                thread.Start();
            };

            WeaverButton.TouchUpInside += delegate
            {
                var thread = new Thread(_dialogController.ShowDialogUsingWeaver);
                thread.Start();
            };
        }

        public void ShowDialog(string text)
        {
            var alertView = new UIAlertView(text, "", null, "Ok", null);
            alertView.Show();
        }
    }
}
