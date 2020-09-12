# About

This is the source code for the statically-generated website [https://www.roclongboarding.info](https://www.roclongboarding.info)

# Building the Website

* Install the [dotnet core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) version 3.1.
* Clone this repo, including submodules.
* Install [cake](https://cakebuild.net/) by running ```dotnet tool install -g Cake.Tool```
* In the repo, run ```dotnet cake --target=build_all```.  This builds [Pretzel](https://github.com/xforever1313/pretzel), the static website generator, the plugin that is used, and creates the site as well.  This only needs to be run once.
* Each time after that, just run ```dotnet cake``` in the root of the repo, and the site will be generated and a browser will open.
