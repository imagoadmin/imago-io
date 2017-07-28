<h1 id="linking-light-table">Linking to the Light Table</h1>

An application can link to imagery and display it using the light table on the Imago web site e.g.

<https://portal.imago.live/api/display?de=AA222&ds=Core%20Boxes&im=Dry&dp=35>

This example searches for a data entity called **AA222** and finds all images of imagery type **Dry** in the **Core Boxes** data series type. These images are then displayed on the light table. The light table is adjusted to show imagery at a depth of **35m**.

## Constructing the URL

The URL must use the address: `https://portal.imago.live/api/display?query`

The following query parameters control how to search for images:

Parameter|Description|Required|
---------|-----------|--------|
 pj | Project name| optional|
 da | Dataset name| optional|
 de | Data entity name|mandatory|
 ds | Data series type name|mandatory|
 im | Imagery type name|mandatory|
 dp | Display at this depth|optional|

If the project or dataset parameters are provided then they will restrict the search for the data entity. Without these parameters, the light table will search across all projects/datasets until it finds a data entity that matches.

Unlike the integration API, no GUID identifiers are used to search for data. Instead, the linking option uses data names. This can lead to conflicts if naming is not unique within the project or dataset. If more than one data entity matches the query name then they are all added to the light table.

All data names are case sensitive.

### Notes

The query must follow the standard definition for URL query parameters i.e. 

`?parameter=value&parameter=value.`

Each value must be URLencoded.
