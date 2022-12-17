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
    // ---------------- Tests ----------------

    [TestMethod]
    public void OutboxJsonLdValidationTest()
    {
        DoValidationTest( "outbox.json", "JSONLD.json" );
    }

    [TestMethod]
    public void FollowingJsonLdValidationTest()
    {
        DoValidationTest( "following.json", "JSONLD.json" );
    }

    [TestMethod]
    public void ProfileJsonLdValidationTest()
    {
        DoValidationTest( "profile.json", "JSONLD.json" );
    }

    [TestMethod]
    public void OutboxActivityPubValidationTest()
    {
        DoValidationTest( "outbox.json", "ActivityPub.json" );
    }

    [TestMethod]
    public void FollowingActivityPubValidationTest()
    {
        DoValidationTest( "following.json", "ActivityPub.json" );
    }

    [TestMethod]
    public void ProfileActivityPubValidationTest()
    {
        DoValidationTest( "profile.json", "ActivityPub.json" );
    }

    // ---------------- Test Helpers ----------------

    private void DoValidationTest( string actPubFileName, string jsonSchemaFileName )
    {
        JToken json = GetJsonToTest( actPubFileName );
        JSchema schema = GetSchema( jsonSchemaFileName );
        
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

    private JSchema GetSchema( string jsonSchemaFileName )
    {
        var schemaDir = new DirectoryInfo(
            Path.Combine(
                TestContants.RepoRoot,
                "_WebsitePlugin",
                "WebsiteTests",
                "ActivityPubSchema"
            )
        );

        var actPubSchemaFile = new FileInfo(
            Path.Combine(
                schemaDir.FullName,
                jsonSchemaFileName
            )
        );

        using( StreamReader schemaFile = File.OpenText( actPubSchemaFile.FullName ) )
        using( var schemaReader = new JsonTextReader( schemaFile ) )
        {
            var resolver = new JSchemaUrlResolver();

            return JSchema.Load(
                schemaReader,
                new JSchemaReaderSettings
                {
                    Resolver = resolver,
                    BaseUri = new Uri( actPubSchemaFile.FullName ),
                    ResolveSchemaReferences = true
                }
            );
        }
    }

    private JToken GetJsonToTest( string actPubFileName )
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
}
