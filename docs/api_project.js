define({
  "name": "Imago IO for Developers",
  "version": "2.0.0",
  "description": "Imago's integration and embedding API.",
  "title": "Imago IO",
  "url": "https://io.imago.live",
  "order": [
    "Session",
    "PostSession",
    "GetSession",
    "DeleteSession",
    "UserContext",
    "Collection",
    "Imagery",
    "Attributes"
  ],
  "header": {
    "title": "Imago IO",
    "content": "<p>Imago extracts value from geoscientific imagery. It consolidates, transforms and delivers imagery into those applications most relevant to each image type. This external integration is achieved via the Imago Io Developer APIs.</p>\n<p>There are two ways to interface with Imago:</p>\n<ul>\n<li>Integration via a Web RESTFul API, or</li>\n<li><a href=\"#linking-light-table\">Linking to images or web views inside your applications.</a></li>\n</ul>\n<h2>Workspaces, Datasets and other Data Structures</h2>\n<p>Imago has a heirarchy to catalogue stored images. This heirarchy is:</p>\n<table>\n<thead>\n<tr>\n<th>Level</th>\n<th>Description</th>\n<th>Example</th>\n</tr>\n</thead>\n<tbody>\n<tr>\n<td>Workspaces</td>\n<td>Groups multiple datasets together.</td>\n<td>Chile Exploration</td>\n</tr>\n<tr>\n<td>Datasets</td>\n<td>A dataset is a group of related collections.</td>\n<td>Pampa Maria, Mia Culpa</td>\n</tr>\n<tr>\n<td>Collections</td>\n<td>A collection manages all images for a single location, object or activity e.g. it could be a drillhole, land tenement, drilling rig, sampling station, etc... It usually has a geographical location associated with it, however this is optional.</td>\n<td>DH001, DH002, LH101</td>\n</tr>\n<tr>\n<td>Imagery Types</td>\n<td>Describes a logical grouping of imagery within the collection.</td>\n<td>Core Trays, Hand Samples, Drone Flyover, Maps</td>\n</tr>\n<tr>\n<td>Imagery</td>\n<td>An imagery is a group of images of the same imagery type on a collection. It usually has an additional geographic reference that identifies its position within the collection, however this is optional e.g. from/to interval, sample location in tenenment, equipment location.</td>\n<td>0m - 3.5m, 3.5m - 7.8m</td>\n</tr>\n<tr>\n<td>Image Types</td>\n<td>Each image is stored against an imagery and identified by its image type.</td>\n<td>Wet, Dry (for core trays) Shift Start, Shift End (for drill rigs)</td>\n</tr>\n</tbody>\n</table>\n<p>In Imago, every peice of data (i.e. workspaces, imagery, etc...) is identified by a GUID identifier e.g.  d699fd33-5453-4a72-bb29-a0613caa16a1.</p>\n<h2>User's Context</h2>\n<p>A user's context is a description of the workspaces, datasets, imagery types and image types that are accessible to them. This description proivdes GUID identifiers that are used through the API to reference different information.</p>\n<h2>Integration API</h2>\n<p>The integration API is a standard Web Service RESTFul API.</p>\n<p>An application must sign into Imago and receive an authorisation token before further API calls are permitted. The authorisation token represents a session and expires after a set period of time.</p>\n<p>Each API call must include the authorisation token using the header &quot;imago-api-token&quot; on the HTTP request.</p>\n<h2>Light Table Linking</h2>\n<p>An application can also link to imagery stored in Imago and display it on the web site's light table.</p>\n"
  },
  "footer": {
    "title": "Linking to Light Table",
    "content": "<h1 id=\"linking-light-table\">Linking to the Light Table</h1>\n<p>An application can link to imagery and display it using the light table on the Imago web site e.g.</p>\n<p><a href=\"https://portal.imago.live/api/display?de=AA222&amp;ds=Core%20Boxes&amp;im=Dry&amp;dp=35\">https://portal.imago.live/api/display?de=AA222&amp;ds=Core Boxes&amp;im=Dry&amp;dp=35</a></p>\n<p>This example searches for a collection called <strong>AA222</strong> and finds all images of image type <strong>Dry</strong> in the <strong>Core Boxes</strong> imagery type. These images are then displayed on the light table. The light table is adjusted to show imagery at a depth of <strong>35m</strong>.</p>\n<p>Alternatively, it can search by collection only. The light table will show the first imagery and image types listed for the matching collection.</p>\n<p><a href=\"https://portal.imago.live/api/display?de=AA222&amp;dp=35\">https://portal.imago.live/api/display?de=AA222&amp;dp=35</a></p>\n<h2>Constructing the URL</h2>\n<p>The URL must use the address: <code>https://portal.imago.live/api/display?query</code></p>\n<p>The following query parameters control how to search for images:</p>\n<table>\n<thead>\n<tr>\n<th>Parameter</th>\n<th>Description</th>\n<th>Required</th>\n</tr>\n</thead>\n<tbody>\n<tr>\n<td>pj</td>\n<td>Workspace name</td>\n<td>optional</td>\n</tr>\n<tr>\n<td>da</td>\n<td>Dataset name</td>\n<td>optional</td>\n</tr>\n<tr>\n<td>de</td>\n<td>Collection name</td>\n<td>mandatory</td>\n</tr>\n<tr>\n<td>ds</td>\n<td>Imagery type name</td>\n<td>optional</td>\n</tr>\n<tr>\n<td>im</td>\n<td>Image type name</td>\n<td>optional</td>\n</tr>\n<tr>\n<td>dp</td>\n<td>Display at this depth</td>\n<td>optional</td>\n</tr>\n<tr>\n<td>ig</td>\n<td>Display at image with this name</td>\n<td>optional</td>\n</tr>\n</tbody>\n</table>\n<p>If the workspace or dataset parameters are provided then they will restrict the search for the collection. Without these parameters, the light table will search across all workspaces/datasets until it finds a collection that matches. If no imagery type or image type is specified then it will display the first imagery/image type listed in the collection's dataset.</p>\n<p>Unlike the integration API, no GUID identifiers are used to search for data. Instead, the linking option uses data names. This can lead to conflicts if naming is not unique within the workspace or dataset. If more than one collection matches the query name then they are all added to the light table.</p>\n<p>All data names are case sensitive.</p>\n<h3>Notes</h3>\n<p>The query must follow the standard definition for URL query parameters i.e.</p>\n<p><code>?parameter=value&amp;parameter=value.</code></p>\n<p>Each value must be URLencoded.</p>\n"
  },
  "sampleUrl": false,
  "defaultVersion": "0.0.0",
  "apidoc": "0.3.0",
  "generator": {
    "name": "apidoc",
    "time": "2020-01-23T02:41:46.859Z",
    "url": "http://apidocjs.com",
    "version": "0.17.7"
  }
});
