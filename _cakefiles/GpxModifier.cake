// This file contains classes that strip all but the important data from GPX files.

#addin nuget:?package=Cake.ArgumentBinder&version=3.0.0

using System.Xml.Linq;

public class GpxModConfig
{
    [StringArgument(
        "input",
        Description = "Path to the GPX File to load and strip away from",
        Required = true
    )]
    public string InputFilePath { get; set; }

    [StringArgument(
        "output",
        Description = "Path here to dump the stripped GPX file (including the file name)",
        Required = true
    )]
    public string OutputFilePath { get; set; }

    [IntegerArgument(
        "start_point",
        Description = "Which point in the GPX to start saving",
        DefaultValue = 0,
        Min = 0,
        Max = int.MaxValue
    )]
    public int StartPoint { get; set; }

    [IntegerArgument(
        "end_point",
        Description = "Which point in the GPX to stop saving",
        DefaultValue = int.MaxValue,
        Min = 0,
        Max = int.MaxValue
    )]
    public int EndPoint { get; set; }
}

Task( "gpx_mod" )
.Does(
    () =>
    {
        GpxModConfig config = CreateFromArguments<GpxModConfig>();

        XDocument doc = XDocument.Load( config.InputFilePath );
        XElement trkElement = doc.Root.Elements().FirstOrDefault( e => "trk".Equals( e.Name.LocalName ) );
        if( trkElement == null )
        {
            throw new InvalidOperationException(
                "Could not find 'trk' element."
            );
        }

        List<XElement> trkPts = new List<XElement>();

        foreach( XElement trkChild in trkElement.Elements() )
        {
            if( "trkseg".Equals( trkChild.Name.LocalName ) )
            {
                foreach( XElement trkseqChild in trkChild.Elements() )
                {
                    if( "trkpt".Equals( trkseqChild.Name.LocalName ) )
                    {
                        trkseqChild.Elements().Where( e => "extensions".Equals( e.Name.LocalName ) ).Remove();
                        trkPts.Add( trkseqChild );
                    }
                }
            }
        }

        int CompareNodes( XElement left, XElement right )
        {
            try
            {
                DateTime leftDate = DateTime.Parse( left.Elements().Where( e => "time".Equals( e.Name.LocalName ) ).FirstOrDefault()?.Value );
                DateTime rightDate = DateTime.Parse( right.Elements().Where( e => "time".Equals( e.Name.LocalName ) ).FirstOrDefault()?.Value );

                return leftDate.CompareTo( rightDate );
            }
            catch( Exception e )
            {
                Information( "Could not parse time from trkpt point.  Does it exist, or is it in the wrong format?" );
                Information( e.Message );
                throw;
            }
        }

        trkPts.Sort( CompareNodes );
        for( int i = 0; i < trkPts.Count; ++i )
        {
            if( ( i < config.StartPoint ) || ( i > config.EndPoint ) )
            {
                trkPts[i].Remove();
            }
        }

        doc.Save( config.OutputFilePath );
    }
).DescriptionFromArguments<GpxModConfig>( "Modifies the given GPX data and strips anything not needed from it." );