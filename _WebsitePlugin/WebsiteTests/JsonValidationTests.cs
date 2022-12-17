//
// Rochester Longboarding Website Plugin - Extensions to Pretzel.
// Copyright (C) 2022 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace WebsiteTests;

[TestClass]
public sealed class JsonValidationTests
{
    // ---------------- Fields ----------------

    private static readonly DirectoryInfo SchemaDir = new DirectoryInfo(
        Path.Combine(
            TestContants.RepoRoot,
            "_WebsitePlugin",
            "WebsiteTests",
            "ActivityPubSchema"
        )
    );

    private static readonly FileInfo ActPubSchemaFile = new FileInfo(
        Path.Combine(
            SchemaDir.FullName,
            "ActivityPub.json"
        )
    );

    private static readonly FileInfo JsonLdSchemaFile = new FileInfo(
        Path.Combine(
            SchemaDir.FullName,
            "JSONLD.json"
        )
    );

    // ---------------- Tests ----------------

    [TestMethod]
    public void OutboxJsonLdValidationTest()
    {
        DoJsonLdSchemaTest( "outbox.json" );
    }

    [TestMethod]
    public void FollowingJsonLdValidationTest()
    {
        DoJsonLdSchemaTest( "following.json" );
    }

    [TestMethod]
    public void ProfileJsonLdValidationTest()
    {
        DoJsonLdSchemaTest( "profile.json" );
    }

    [TestMethod]
    public void OutboxActivityPubValidationTest()
    {
        DoActivityPubValidationTest( "outbox.json" );
    }

    [TestMethod]
    public void FollowingActivityPubValidationTest()
    {
        DoActivityPubValidationTest( "following.json" );
    }

    [TestMethod]
    public void ProfileActivityPubValidationTest()
    {
        DoActivityPubValidationTest( "profile.json" );
    }

    // ---------------- Test Helpers ----------------

    private void DoJsonLdSchemaTest( string actPubFileName )
    {
        JToken json = GetJsonToTest( actPubFileName );
        JSchema schema = GetJsonLdSchema();

        Validate( json, schema );
    }

    private void DoActivityPubValidationTest( string actPubFileName )
    {
        JToken json = GetJsonToTest( actPubFileName );
        JSchema schema = GetActPubSchema();

        Validate( json, schema );
    }

    private static JSchema GetJsonLdSchema()
    {
        using( StreamReader schemaFile = File.OpenText( JsonLdSchemaFile.FullName ) )
        using( var schemaReader = new JsonTextReader( schemaFile ) )
        {
            return JSchema.Load(
                schemaReader,
                new JSchemaReaderSettings
                {
                    ResolveSchemaReferences = true
                }
            );
        }
    }

    private static JSchema GetActPubSchema()
    {
        var typeDir = new DirectoryInfo(
            Path.Combine( SchemaDir.FullName, "type" )
        );

        var resolver2 = new JSchemaPreloadedResolver();
        resolver2.Add(
            new Uri( "http://www.w3.org/ns/activitystreams/JSONLD.json" ),
            File.ReadAllBytes( JsonLdSchemaFile.FullName )
        );

        foreach( FileInfo file in typeDir.GetFiles( "*.json" ) )
        {
            var uri = new Uri( @$"http://www.w3.org/ns/activitystreams/{file.Name}", UriKind.RelativeOrAbsolute );
            resolver2.Add(
                uri,
                File.ReadAllBytes( file.FullName )
            );
        }

        using( StreamReader schemaFile = File.OpenText( ActPubSchemaFile.FullName ) )
        using( var schemaReader = new JsonTextReader( schemaFile ) )
        {
            return JSchema.Load(
                schemaReader,
                new JSchemaReaderSettings
                {
                    Resolver = resolver2,
                    ResolveSchemaReferences = true
                }
            );
        }
    }

    private static JToken GetJsonToTest( string actPubFileName )
    {
        var fileToCheck = new FileInfo(
            Path.Combine( TestContants.SiteOutput, "activitypub", actPubFileName )
        );

        Assert.IsTrue( fileToCheck.Exists, $"{fileToCheck.FullName} does not exist! Was the site built?" );

        using( StreamReader fileReader = File.OpenText( fileToCheck.FullName ) )
        using( var fileTextReader = new JsonTextReader( fileReader ) )
        {
            return JToken.Load( fileTextReader );
        }
    }

    private static void Validate( JToken json, JSchema schema )
    {
        IList<string> errorMessages;
        bool isValid = json.IsValid( schema, out errorMessages );
        if( isValid == false )
        {
            var builder = new StringBuilder();
            foreach( string error in errorMessages )
            {
                builder.AppendLine( "- " + error );
            }
            Assert.Fail( builder.ToString() );
        }
    }
}
