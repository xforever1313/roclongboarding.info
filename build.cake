string target = Argument( "target", "taste" );

const string gpsDataFolder = "./gpxdata";

#load "_cakefiles/GpxModifier.cake"

const string pretzelExe = "./_pretzel/src/Pretzel/bin/Debug/netcoreapp3.1/Pretzel.dll";
const string pluginDir = "./_plugins";
const string categoryPlugin = "./_plugins/Pretzel.Categories.dll";
const string mapPlugin = "./_plugins/MapPlugin.dll";

Task( "taste" )
.Does(
    () =>
    {
        RunPretzel( "taste" );
    }
).Description( "Calls pretzel taste to try the site locally" );


Task( "generate" )
.Does(
    () =>
    {
        EnsureDirectoryExists( "_site" );
        CleanDirectory( "_site" );
        RunPretzel( "bake" );
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

    DotNetCoreBuildSettings settings = new DotNetCoreBuildSettings
    {
        Configuration = "Debug"
    };

    DotNetCoreBuild( "./_pretzel/src/Pretzel.sln", settings );

    EnsureDirectoryExists( pluginDir );
    FilePathCollection categoryFiles = GetFiles( "./_pretzel/src/Pretzel.Categories/bin/Debug/netstandard2.1/Pretzel.Categories.*" );
    CopyFiles( categoryFiles, Directory( pluginDir ) );

    Information( "Building Pretzel... Done!" );
}

void BuildPlugin()
{
    CheckPretzelDependency();

    Information( "Building Plugin..." );

    DotNetCorePublishSettings  settings = new DotNetCorePublishSettings
    {
        Configuration = "Debug",
        NoBuild = false,
        NoRestore = false,
        PublishTrimmed = true
    };

    DotNetCorePublish( "./_MapPlugin/MapPlugin.sln", settings );

    EnsureDirectoryExists( pluginDir );
    FilePathCollection files = GetFiles( "./_MapPlugin/MapPlugin/bin/Debug/netstandard2.1/publish/MapPlugin.*" );
    CopyFiles( files, Directory( pluginDir ) );

    files = GetFiles( "./_MapPlugin/MapPlugin/bin/Debug/netstandard2.1/publish/Geodesy.*" );
    CopyFiles( files, Directory( pluginDir ) );

    Information( "Building Plugin... Done!" );
}

void RunPretzel( string argument )
{
    CheckPretzelDependency();
    CheckMapPluginDependency();

    ProcessSettings settings = new ProcessSettings
    {
        Arguments = ProcessArgumentBuilder.FromString( $"\"{pretzelExe}\" {argument} --debug" ),
        Silent = false
    };

    int exitCode = StartProcess( "dotnet", settings );
    if( exitCode != 0 )
    {
        throw new Exception( $"Pretzel exited with exit code: {exitCode}" );
    }
}

void CheckPretzelDependency()
{
    if( ( FileExists( pretzelExe ) == false ) || ( FileExists( categoryPlugin ) == false ) )
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
