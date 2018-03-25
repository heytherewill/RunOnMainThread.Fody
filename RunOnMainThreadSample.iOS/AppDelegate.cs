using Foundation;
using UIKit;

namespace RunOnMainThreadSample.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }
    }
}

