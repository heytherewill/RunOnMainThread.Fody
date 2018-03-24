var target = Argument("target", "Default");

const string targetProject = "./RunOnMainThread.sln";
const string nuspecFile = "./RunOnMainThread.nuspec";
var apiKey = EnvironmentVariable("NUGET_API_KEY");

var packSettings = new NuGetPackSettings();
var buildSettings = new MSBuildSettings 
{
    Verbosity = Verbosity.Verbose,
    Configuration = "Release"
};

var publishSettings = new NuGetPushSettings 
{
    ApiKey = apiKey,
    Source = "https://www.nuget.org/api/v2/package", 
};

Task("Restore")
    .Does(() => NuGetRestore(targetProject));

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => MSBuild(targetProject, buildSettings));

Task("Copy")
    .IsDependentOn("Build")
    .Does(() =>
    {
        //Setup
        EnsureDirectoryExists("build/lib/netstandard2.0");
        EnsureDirectoryExists("build/lib/MonoAndroid10");
        EnsureDirectoryExists("build/lib/Xamarin.iOS10");
        EnsureDirectoryExists("build/lib/Xamarin.Mac20");

        //netstandard
        CopyFiles(@"bin\Release\netstandard2.0\*.dll", @"build\lib\netstandard2.0");

        //Android
        CopyFiles(@"bin\Release\netstandard2.0\*.dll", @"build\lib\MonoAndroid10");
        CopyFiles(@"bin\Release\RunOnMainThread.Droid.dll", @"build\lib\MonoAndroid10");

        //iOS
        CopyFiles(@"bin\Release\netstandard2.0\*.dll", @"build\lib\Xamarin.iOS10");
        CopyFiles(@"bin\Release\RunOnMainThread.iOS.dll", @"build\lib\Xamarin.iOS10");

        //macOS
        CopyFiles(@"bin\Release\netstandard2.0\*.dll", @"build\lib\Xamarin.Mac20");
        CopyFiles(@"bin\Release\RunOnMainThread.macOS.dll", @"build\lib\Xamarin.Mac20");
    });

Task("Pack")
    .IsDependentOn("Copy")
    .Does(() => NuGetPack(nuspecFile, packSettings));

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => 
    {
        try
        {
            NuGetPush(GetFiles("*.nupkg").First().FullPath, publishSettings);
        }
        catch(CakeException) { /* Swallow exception if package already exists */}
    });

//Default Operation
Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);