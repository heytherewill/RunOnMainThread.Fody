using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading;
using RunOnMainThreadSample.Core;

namespace RunOnMainThreadSample.Droid
{
    [Activity(Label = "RunOnMainThreadSample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IDialogDisplayer
    {
        private DialogController _dialogController;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.CrashButton).Click += delegate 
            {
                var thread = new Thread(_dialogController.ShowDialog);
                thread.Start();
            };

            FindViewById<Button>(Resource.Id.DispatcherButton).Click += delegate 
            {
                var thread = new Thread(_dialogController.ShowDialogUsingDispatcher);
                thread.Start();
            };

            FindViewById<Button>(Resource.Id.WeaverButton).Click += delegate 
            {
                var thread = new Thread(_dialogController.ShowDialogUsingWeaver);
                thread.Start();
            };
        }

        public void ShowDialog(string text)
        {
            var builder = new AlertDialog.Builder(this)
                .SetMessage(text)
                .SetPositiveButton("Ok", (s, e) => { });

            builder.Show();
        }
    }
}

