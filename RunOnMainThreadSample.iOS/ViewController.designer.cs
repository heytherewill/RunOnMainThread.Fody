// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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