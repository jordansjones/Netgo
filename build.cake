///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Debug");
var outputDirectory = Directory(Argument<string>("OutDir", Context.Environment.WorkingDirectory.Combine("publish").FullPath)).Path.MakeAbsolute(Context.Environment.WorkingDirectory);

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = Context.Environment.WorkingDirectory.GetFilePath("NGo.sln");
var runtimeIdentifiers = new string[] { "win-x64", "linux-x64", "osx-x64" };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    // Executed BEFORE the first task.
});

Teardown(context =>
{
    // Executed AFTER the last task.
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Build").Does(() => 
{
    DotNetCoreBuild(solutionPath.FullPath, new DotNetCoreBuildSettings {
        Configuration = configuration
    });
});

Task("Publish")
.IsDependentOn("Default")
.Does(() => 
{
    foreach (var rid in runtimeIdentifiers)
    {
        var outputDir = outputDirectory.Combine(rid);
        if (DirectoryExists(outputDir))
        {
            DeleteDirectory(outputDir, new DeleteDirectorySettings {
                Force = true,
                Recursive = true
            });
        }
        DotNetCorePublish(solutionPath.FullPath, new DotNetCorePublishSettings {
            Configuration = configuration,
            OutputDirectory = outputDir,
            Runtime = rid,
            SelfContained = true
        });
    }
});

Task("Default")
    .IsDependentOn("Build");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);