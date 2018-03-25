using Android.App;
using Android.Widget;
using Android.OS;
using RunOnMainThread;
using System.Threading;

namespace RunOnMainThreadSample.Droid
{
    [Activity(Label = "RunOnMainThreadSample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.CrashButton).Click += delegate 
            {
                var thread = new Thread(ShowDialog);
                thread.Start();
            };

            FindViewById<Button>(Resource.Id.DispatcherButton).Click += delegate 
            {
                var thread = new Thread(ShowDialogUsingDispatcher);
                thread.Start();
            };

            FindViewById<Button>(Resource.Id.WeaverButton).Click += delegate 
            {
                var thread = new Thread(ShowDialogUsingWeaver);
                thread.Start();
            };
        }

        private void ShowDialog()
        {
            var builder = new AlertDialog.Builder(this)
                .SetMessage("This runs on the UI thread!")
                .SetPositiveButton("Ok", (s, e) => { });

            builder.Show();
        }
        private void ShowDialogUsingDispatcher()
        {
            MainThreadDispatcher.RunOnMainThread(() =>
            {
                var builder = new AlertDialog.Builder(this)
                    .SetMessage("This runs on the UI thread!")
                    .SetPositiveButton("Ok", (s, e) => { });

                builder.Show();
            });
        }

        [RunOnMainThread]
        private void ShowDialogUsingWeaver()
        {
            var builder = new AlertDialog.Builder(this)
                .SetMessage("This runs on the UI thread!")
                .SetPositiveButton("Ok", (s, e) => { });

            builder.Show();
        }
    }
}

