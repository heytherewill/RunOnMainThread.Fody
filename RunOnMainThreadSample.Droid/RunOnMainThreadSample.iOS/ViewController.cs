using System;
using System.Threading;
using RunOnMainThread;
using UIKit;

namespace RunOnMainThreadSample.iOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) 
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CrashButton.TouchUpInside += delegate 
            {
                var thread = new Thread(ShowDialog);
                thread.Start();
            };

            DispatcherButton.TouchUpInside += delegate
            {
                var thread = new Thread(ShowDialogUsingDispatcher);
                thread.Start();
            };

            WeaverButton.TouchUpInside += delegate
            {
                var thread = new Thread(ShowDialogUsingWeaver);
                thread.Start();
            };
        }

        private void ShowDialog()
        {
            var alertView = new UIAlertView("This runs on the UI thread!", "", null, "Ok", null);
            alertView.Show();
        }

        private void ShowDialogUsingDispatcher()
        {
            MainThreadDispatcher.RunOnMainThread(() =>
            {
                var alertView = new UIAlertView("This runs on the UI thread!", "", null, "Ok", null);
                alertView.Show();
            });
        }

        [RunOnMainThread]
        private void ShowDialogUsingWeaver()
        {
            var alertView = new UIAlertView("This runs on the UI thread!", "", null, "Ok", null);
            alertView.Show();
        }
    }
}
