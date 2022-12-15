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

namespace WebsiteTests;

[TestClass]
public sealed class JsonValidationTests
{
    // ---------------- Tests ----------------

    [TestMethod]
    public void OutboxValidationTest()
    {
        DoValidationTest( "outbox.json" );
    }

    [TestMethod]
    public void FollowingValidationTest()
    {
        DoValidationTest( "following.json" );
    }

    [TestMethod]
    public void ProfileValidationTest()
    {
        DoValidationTest( "profile.json" );
    }

    // ---------------- Test Helpers ----------------

    private void DoValidationTest( string actPubFileName )
    {
        FileInfo info = new FileInfo(
            Path.Combine( TestContants.SiteOutput, "activitypub", actPubFileName )
        );

        Assert.IsTrue( info.Exists, $"{info.FullName} does not exist! Was the site built?" );
    }
}
