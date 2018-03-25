// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RunOnMainThreadSample.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton CrashButton { get; set; }

		[Outlet]
		UIKit.UIButton DispatcherButton { get; set; }

		[Outlet]
		UIKit.UIButton WeaverButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CrashButton != null) {
				CrashButton.Dispose ();
				CrashButton = null;
			}

			if (DispatcherButton != null) {
				DispatcherButton.Dispose ();
				DispatcherButton = null;
			}

			if (WeaverButton != null) {
				WeaverButton.Dispose ();
				WeaverButton = null;
			}
		}
	}
}
