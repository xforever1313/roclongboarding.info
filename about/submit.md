---
layout : aboutpage
title : Submit
icon: fa-upload
description: "How to submit places to board"
tags: [longboarding, rochester, about, roclongboarding.org, longboard, ny, new york, submit, github]
pageindex: 2
---

Want to add a location to the site?  There are several ways to do it!

## Submission Criteria

A location should be safe and practical to longboard on.  For example, dirt paths or gravel probably aren't good locations.  In addition, it is best to avoid busy streets to prevent accidents,  Ideally, we want places with little or even no car traffic.

A location should be legal for the general public to access.  If someone needs to break trespassing laws to get access to a location, it is not a good location.

## How to Submit

### Tell us about a location

The easiest thing to do is drop us a tip on where to go longboarding.  You just need to send the location to our email at ```@Model.Site.Config["contact"]```.

You can also submit an issue to our [GitHub](@Model.Site.Config["githubissues"]) with a suggestion if you have an account there.

If you want your name or username to be included in the location post under a "Suggested By" section, indicate so in the submission.  If you have a website, social media, whatever you want to associate with yourself, include that as well, and we'll link it to your name under the "Suggest By" section.  If you do not explicitly indicate that your name under the "Suggested By" section, we'll assume that you do not want to.

### Submit a Guest Post

Please keep in mind the following when submitting a guest post:
* It may take time for us to process a submission.
* Your submission can be rejected for any reason.
* If you write a guest post, all posts are [CC BY-SA](https://creativecommons.org/licenses/by-sa/4.0/).  If you are not comfortable releasing your posts under this license, do not submit a guest post.
* You can request a deletion of a guest post you wrote by sending an email WITH THE SAME EMAIL to ```@Model.Site.Config["contact"]``` and asking us to remove it.  Or, put in a GitHub issue with the same account used to make a pull request or submit an issue to.  However, keep in mind that this site is powered by a public [Git](https://en.wikipedia.org/wiki/Git) repository, and by the nature of Git, once something is committed into it, it can never go away.  The post is there forever in the Git history, and anyone could have cloned it at any time.  If you are not comfortable with this, please do not submit a guest post.
* Any photos you must own the copyright to, and also must be [CC BY-SA](https://creativecommons.org/licenses/by-sa/4.0/).
* YouTube videos, are allowed embedded and they can be the standard YouTube license.
* [GPX](https://en.wikipedia.org/wiki/GPS_Exchange_Format) data if you want to include the area on a map.

Posts can include things such as:
* Hazards (things to avoid)
* Places of Interest (things to checkout)
* Videos
* Photos
* A description of the area
* Is the area Cool, Meh, or Lame

All posts are in the [Markdown](https://en.wikipedia.org/wiki/Markdown) format with some metadata.  You can see an example of what this looks like [here](https://raw.githubusercontent.com/xforever1313/roclongboarding.info/master/_posts/2020-9-7-ErieCanalClinton.md?token=AATVIXUI322VIKQKU6RYUVC7LUXX6).  The metadata at the top is important, as it allows us to plot information on the map.  Below are all the the metadata that must be included in a post:

**Required Metadata**
* **layout:** should always be set to "location".
* **title:** The location that is being written about.
* **author:** Your username or real name; up to you.
* **comments:** Should be set to "true".
* **category:** Should be set to "Cool Places" if the place is fun to longboard, "Meh Places" if the place is not-so-fun to longboard, or "Lame Places" if the place is not a good longboarding spot for whatever reason.
* **description:** A brief, one-line description of the place that is being written about.
* **tags:** What to tag the location if someone were trying to search for it.  For example, if the location is along the Erie Canal Trail, "Erie Canal Trail" could be a tag.
* **display_map:** If you have GPX data or want to map areas on a map within the post, set this to true, otherwise set it to false.
* **display_elevation:** If you have elevation data, set this to true, otherwise false.

**Metadata if "display_map" is set to true**
* **center_point:** Where the marker is placed on the [BigMap](/bigmap.html) for the location.  It is a latitude, longitude pair.  This is required.
* **points_of_interest:** Any locations that someone should checkout.  Must include a name and a latitude/longitude pair.
* **hazards:** Any locations that someone should avoid or be cautious of.  Must include a name and a latitude/longitude pair.
* **cool_lines:** Areas that are safe and fun to longboard on.  Can either be GPX data or manually entering coordinates.  If "display_elevation" is set to false, then the manually-entered coordinates are just latitude/longitude pairs, but if set to on it is a latitude/longitude/elevation set.  Must include a name of the segment and the accompanying GPX file or manually-entered GPS coordinates.
* **meh_lines:** Same as cool_lines other than these areas are not-so-fun to longboard or are boring.
* **lame_lines:** Same as cool_lines and meh_lines other than these areas are dangerous or impossible to longboard on.
* **cool_polys:** Polygons are areas that are filled in.  You can use this to mark entire areas as cool, meh, or lame.  Like cool_lines, they require a name and manually-entered GPS coordinates.  GPX data is not supported with polygons.  Elevation data is also ignored, so coordinates are a latitude/longitude pair.  Cool polygons can be entire areas (e.g. big empty parking lots) that are safe to longboard on.
* **meh_polys:** Same has cool_polys other than these areas are not-so-fun to longboard or are boring.
* **lame_polys:**  Same as cool_polys and meh_polys other than these areas are dangerous or impossible to longboard on.

An example of all of this:
```
---
layout: location
title: "Some Cool Location"
author: "Seth Hendrick"
comments: true
category: "Cool Places"
description: "This is a pretty cool spot"
tags: [Cool Spot, Henrietta, Downhill, Paved]

display_map: true
display_elevation: false
center_point: [1, 2]

points_of_interest:
    - name: poi1
      coord: [10, 11]
    - name: poi2
      coord: [111, 112]

hazards:
    - name: haz1
      coord: [3, 4]
    - name: haz2
      coord: [5, 6]
    
cool_lines:
    - name: default
      coords: "./_gpxdata/default.gpx"

meh_lines:
    - name: bumpy_road
      coords: [[5, 6], [7, 8]]
    
lame_lines:
    - name: dirt_road_with_elevation
      coords: [[7, 8, 0], [9, 10, 1]]
    - name: rocky road
      coords: "./_gpxdata/rocky.gpx"

cool_polys:
  - name: parking lot 1
    coords: [[100, 100], [101, 101], [102, 102]]

meh_polys:
  - name: parking lot 2
    coords: [[110, 110], [111, 111], [112, 112]]

lame_polys:
    - name: Bumpy Lottion
      coords: [[37, 38], [39, 40], [41, 42]]
    - name: Rocky Lot
      coords: [[47, 48], [49, 50], [51, 52]]
---

Your content here!

```

One you have your Markdown file created, name it ```yyyy-mm-dd-Location.md```, where yyyy is the year, mm is the month, and dd is the day of the month.  You can then either email it to ```@Model.Site.Config["contact"]``` or submit a pull request on [GitHub](@Model.Site.Config["github"]) by placing the new markdown file in the ```_posts``` folder.

If you want to run the site locally before submitting, please follow the instructions on our readme on [GitHub](@Model.Site.Config["github"]).
