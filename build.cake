#tool nuget:?package=vswhere

var target = Argument("target", "Default");

const string targetProject = "./RunOnMainThread.sln";
const string nuspecFile = "./RunOnMainThread.nuspec";
var apiKey = EnvironmentVariable("NUGET_API_KEY");

var msBuildPath = findMSBuildPath();
var packSettings = new NuGetPackSettings();
var buildSettings = new MSBuildSettings 
{
    ToolPath = msBuildPath,
    Verbosity = Verbosity.Verbose,
    Configuration = "Release"
};
var publishSettings = new NuGetPushSettings 
{
    ApiKey = apiKey,
    Source = "https://www.nuget.org/api/v2/package", 
};

private FilePath findMSBuildPath()
{
    FilePath msBuildPath = null;

    if (IsRunningOnWindows())
    {
        var vsLatest = VSWhereLatest(new VSWhereLatestSettings
        {
            IncludePrerelease = true,
            Requires = "Component.Xamarin"
        });

        if (vsLatest != null)
        {
            msBuildPath = vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
        }
    }

    return msBuildPath;
}

Task("Clean")
    .Does(() => 
    {
        CleanDirectory("./bin");
        CleanDirectory("./build");
        CleanDirectory("./packages");
        CleanDirectory("./RunOnMainThread/bin");
        CleanDirectory("./RunOnMainThread/obj");
        CleanDirectory("./RunOnMainThread.Fody/bin");
        CleanDirectory("./RunOnMainThread.Fody/obj");
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
        EnsureDirectoryExists("build/netclassicweaver");
        EnsureDirectoryExists("build/netstandardweaver");
  
        var targets = new [] { "netstandard2.0", "net461", "MonoAndroid81", "Xamarin.iOS10", "Xamarin.Mac20", "Xamarin.TvOS10", "Xamarin.WatchOS10" };
      
        foreach(var target in targets)
        {
            EnsureDirectoryExists($"build/lib/{target}");
            CopyFiles($@"bin\Release\{target.ToLower()}\RunOnMainThread.dll", $@"build\lib\{target}");
        }

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
    .IsDependentOn("Pack");

RunTarget(target);