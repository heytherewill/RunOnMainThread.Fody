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

Task("Clean")
    .Does(() => 
    {
        CleanDirectory("./bin");
        CleanDirectory("./build");
        CleanDirectory("./packages");
        CleanDirectory("./RunOnMainThread/bin");
        CleanDirectory("./RunOnMainThread/obj");
        CleanDirectory("./RunOnMainThread.iOS/bin");
        CleanDirectory("./RunOnMainThread.iOS/obj");
        CleanDirectory("./RunOnMainThread.Fody/bin");
        CleanDirectory("./RunOnMainThread.Fody/obj");
        CleanDirectory("./RunOnMainThread.macOS/bin");
        CleanDirectory("./RunOnMainThread.macOS/obj");
        CleanDirectory("./RunOnMainThread.Droid/bin");
        CleanDirectory("./RunOnMainThread.Droid/obj");
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => NuGetRestore(targetProject));

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => MSBuild(targetProject, buildSettings));

Task("Copy")
    .IsDependentOn("Build")
    .Does(() =>
    {
        //Setup
        EnsureDirectoryExists("build/netclassicweaver");
        EnsureDirectoryExists("build/netstandardweaver");
        EnsureDirectoryExists("build/lib/MonoAndroid10");
        EnsureDirectoryExists("build/lib/Xamarin.iOS10");
        EnsureDirectoryExists("build/lib/Xamarin.Mac20");
        EnsureDirectoryExists("build/lib/netstandard2.0");
        
        //netstandard
        CopyFiles(@"RunOnMainThread\bin\Release\netstandard2.0\RunOnMainThread.dll", @"build\lib\netstandard2.0");

        //Android
        CopyFiles(@"RunOnMainThread.Droid\bin\Release\RunOnMainThread.dll", @"build\lib\MonoAndroid10");

        //iOS
        CopyFiles(@"RunOnMainThread.iOS\bin\Release\RunOnMainThread.dll", @"build\lib\Xamarin.iOS10");

        //macOS
        CopyFiles(@"RunOnMainThread.macOS\bin\Release\RunOnMainThread.dll", @"build\lib\Xamarin.Mac20");

        //Weavers
        CopyFiles(@"bin\Release\net46\RunOnMainThread.Fody.dll", @"build\netclassicweaver");
        CopyFiles(@"bin\Release\net46\RunOnMainThread.Fody.pdb", @"build\netclassicweaver");
        CopyFiles(@"bin\Release\netstandard2.0\RunOnMainThread.Fody.dll", @"build\netstandardweaver");
        CopyFiles(@"bin\Release\netstandard2.0\RunOnMainThread.Fody.pdb", @"build\netstandardweaver");
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