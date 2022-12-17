# About

This is the source code for the statically-generated website [https://www.roclongboarding.info](https://www.roclongboarding.info)

## Building the Website

* Install the [dotnet core SDK](https://dotnet.microsoft.com/download/dotnet-core/6.0) version 6.0.
* Clone this repo, including submodules.
* Install [cake](https://cakebuild.net/) by running ```dotnet tool install -g Cake.Tool```
* In the repo, run ```dotnet cake --target=build_all```.  This builds [Pretzel](https://github.com/xforever1313/pretzel), the static website generator, the plugin that is used, and creates the site as well.  This only needs to be run once.
* Each time after that, just run ```dotnet cake``` in the root of the repo, and the site will be generated and a browser will open.

## License

* Website contents are licensed under [CC-by-sa 4.0](https://creativecommons.org/licenses/by-sa/4.0/).
* Code located in _WebsitePlugin is licensed under [AGPL 3.0](https://www.gnu.org/licenses/agpl-3.0.en.html)
  * Prior to commit [e67aec9792b1db3829b0db3f63d429957ce4e0eb](https://github.com/xforever1313/roclongboarding.info/commit/e67aec9792b1db3829b0db3f63d429957ce4e0eb), the source files in the _MapPlugin were licensed under the [Boost Software License 1.0](https://www.boost.org/users/license.html).

## Dependencies

These are the submodules that this repo depends on.

* **_includes** - [StaticWebsitesCommon](https://github.com/xforever1313/StaticWebsitesCommon)
* **_pretzel** - [Forked version of Pretzel](https://github.com/xforever1313/pretzel)
* **_pretzel/src/ActivityStreams** - [Forked version of ActivityStreams](https://github.com/xforever1313/ActivityStreams)
* **_WebsitePlugin/WebsiteTests/ActivityPubSchema** - [ActivityStream Schema](https://github.com/redaktor/ActivityPubSchema)
