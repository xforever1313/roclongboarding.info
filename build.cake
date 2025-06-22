string target = Argument( "target", "taste" );
string testResultOutput = Argument( "test_result_dir", "_TestResults" );

const string runtime = "net8.0";

const string gpsDataFolder = "./gpxdata";

#load "_cakefiles/GpxModifier.cake"

const string pretzelExe = $"./_pretzel/src/Pretzel/bin/Debug/{runtime}/Pretzel.dll";
const string pluginDir = "./_plugins";
const string siteDir = "./_plugins";
const string categoryPlugin = "./_plugins/Pretzel.Categories.dll";
const string extensionPlugin = "./_plugins/Pretzel.SethExtensions.dll";
const string activityPubPlugin = "./_plugins/KristofferStrube.ActivityStreams.dll";
const string mapPlugin = "./_plugins/MapPlugin.dll";

var msBuildSettings = new DotNetMSBuildSettings();
msBuildSettings.Properties["DefineConstants"] = new List<string> { "NO_EXPORT_SETH_MARKDOWN_ENGINE" };

Task( "taste" )
.Does(
    () =>
    {
        RunPretzel( "taste", false );
    }
).Description( "Calls pretzel taste to try the site locally" );


Task( "generate" )
.Does(
    () =>
    {
        EnsureDirectoryExists( siteDir );
        CleanDirectory( siteDir );
        RunPretzel( "bake", true );
    }
).Description( "Builds the site for publishing." );

Task( "build_pretzel" )
.Does(
    () =>
    {
        BuildPretzel();
    }
).Description( "Compiles Pretzel" );

Task( "build_plugin" )
.Does(
    () =>
    {
        BuildPlugin();
    }
).Description( "Builds the map plugin" );

Task( "build_all" )
.IsDependentOn( "build_pretzel" )
.IsDependentOn( "build_plugin")
.IsDependentOn( "taste" );

void BuildPretzel()
{
    Information( "Building Pretzel..." );

    var settings = new DotNetBuildSettings
    {
        Configuration = "Debug",
        MSBuildSettings = msBuildSettings
    };

    DotNetBuild( "./_pretzel/src/Pretzel.sln", settings );

    EnsureDirectoryExists( pluginDir );

    // Move Pretzel.Categories.
    {
        FilePathCollection files = GetFiles( $"./_pretzel/src/Pretzel.Categories/bin/Debug/{runtime}/Pretzel.Categories.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    // Move Pretzel.SethExtensions
    {
        FilePathCollection files = GetFiles( $"./_pretzel/src/Pretzel.SethExtensions/bin/Debug/{runtime}/Pretzel.SethExtensions.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    // Move ActivityPub
    {
        FilePathCollection files = GetFiles( $"./_pretzel/src/Pretzel.SethExtensions/bin/Debug/{runtime}/KristofferStrube.ActivityStreams.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    Information( "Building Pretzel... Done!" );
}

void BuildPlugin()
{
    CheckPretzelDependency();

    Information( "Building Plugin..." );

    DotNetPublishSettings  settings = new DotNetPublishSettings
    {
        Configuration = "Debug",
        NoBuild = false,
        NoRestore = false,
        MSBuildSettings = msBuildSettings
    };

    DotNetPublish( "./_WebsitePlugin/MapPlugin/MapPlugin.csproj", settings );

    EnsureDirectoryExists( pluginDir );
    FilePathCollection files = GetFiles( $"./_WebsitePlugin/MapPlugin/bin/Debug/{runtime}/publish/MapPlugin.*" );
    CopyFiles( files, Directory( pluginDir ) );

    files = GetFiles( $"./_WebsitePlugin/MapPlugin/bin/Debug/{runtime}/publish/Geodesy.*" );
    CopyFiles( files, Directory( pluginDir ) );

    Information( "Building Plugin... Done!" );
}

void RunPretzel( string argument, bool abortOnFail )
{
    CheckPretzelDependency();
    CheckMapPluginDependency();

    bool fail = false;
    string onStdOut( string line )
    {
        if( string.IsNullOrWhiteSpace( line ) )
        {
            return line;
        }
        else if( line.StartsWith( "Failed to render template" ) )
        {
            fail = true;
        }

        Console.WriteLine( line );

        return line;
    }

    ProcessSettings settings = new ProcessSettings
    {
        Arguments = ProcessArgumentBuilder.FromString( $"\"{pretzelExe}\" {argument} --debug" ),
        Silent = false,
        RedirectStandardOutput = abortOnFail,
        RedirectedStandardOutputHandler = onStdOut
    };

    int exitCode = StartProcess( "dotnet", settings );
    if( exitCode != 0 )
    {
        throw new Exception( $"Pretzel exited with exit code: {exitCode}" );
    }

    if( abortOnFail && fail )
    {
        throw new Exception( "Failed to render template" );   
    }
}

void CheckPretzelDependency()
{
    if(
        ( FileExists( pretzelExe ) == false ) ||
        ( FileExists( categoryPlugin ) == false ) ||
        ( FileExists( extensionPlugin ) == false ) ||
        ( FileExists( activityPubPlugin ) == false )
    )
    {
        BuildPretzel();
    }
}

void CheckMapPluginDependency()
{
    if( FileExists( mapPlugin ) == false )
    {
        BuildPlugin();
    }
}

RunTarget( target );
