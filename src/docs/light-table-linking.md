<h1 id="linking-light-table">Linking to the Light Table</h1>

An application can link to imagery and display it using the light table on the Imago web site e.g.

<https://portal.imago.live/api/display?de=AA222&ds=Core%20Boxes&im=Dry&dp=35>

This example searches for a collection called **AA222** and finds all images of image type **Dry** in the **Core Boxes** imagery type. These images are then displayed on the light table. The light table is adjusted to show imagery at a depth of **35m**.

Alternatively, it can search by collection only. The light table will show the first imagery and image types listed for the matching collection.

<https://portal.imago.live/api/display?de=AA222&dp=35>

## Constructing the URL

The URL must use the address: `https://portal.imago.live/api/display?query`

The following query parameters control how to search for images:

Parameter|Description|Required|
---------|-----------|--------|
 pj | Workspace name| optional|
 da | Dataset name| optional|
 de | Collection name|mandatory|
 ds | Imagery type name|optional|
 im | Image type name|optional|
 dp | Display at this depth|optional|

If the workspace or dataset parameters are provided then they will restrict the search for the collection. Without these parameters, the light table will search across all workspaces/datasets until it finds a collection that matches. If no imagery type or image type is specified then it will display the first imagery/image type listed in the collection's dataset.

Unlike the integration API, no GUID identifiers are used to search for data. Instead, the linking option uses data names. This can lead to conflicts if naming is not unique within the workspace or dataset. If more than one collection matches the query name then they are all added to the light table.

All data names are case sensitive.

### Notes

The query must follow the standard definition for URL query parameters i.e. 

`?parameter=value&parameter=value.`

Each value must be URLencoded.

