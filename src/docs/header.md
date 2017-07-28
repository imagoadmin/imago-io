Imago extracts value from geoscientific imagery. It consolidates, transforms and delivers imagery into those applications most relevant to each image type. This external integration is achieved via the Imago Io Developer APIs.

There are two ways to interface with Imago:

* Integration via a Web RESTFul API, or
* [Linking to images or web views inside your applications.](#linking-light-table)

## Projects, Datasets and other Data Structures

Imago has a heirarchy to catalogue stored images. This heirarchy is:

|Level        | Description |    Example  |
|-------------|-------------|-------------|
|Projects | Groups multiple datasets together. | Chile Exploration|
|Datasets | A dataset is a collection of data entities.| Pampa Maria, Mia Culpa|
|Data Entities | A data entity manages all images for a single location, object or activity e.g. it could be a drillhole, land tenement, drilling rig, sampling station, etc... It usually has a geographical location associated with it, however this is optional.    | DH001, DH002, LH101|
|Data Series Types |Describes a logical grouping of imagery within the data entity. | Core Trays, Hand Samples, Drone Flyover, Maps |
|Data Items | A data item is a collection of images of the same data series type on a data entity. It usually has an additional geographic reference that identifies its position within the data entity, however this is optional e.g. from/to interval, sample location in tenenment, equipment location. | 0m - 3.5m, 3.5m - 7.8m |
|Imagery Types | Each image is stored against a data item and identified by its imagery type. | Wet, Dry (for core trays) Shift Start, Shift End (for drill rigs) |

In Imago, every peice of data (i.e. projects, data items, etc...) is identified by a GUID identifier e.g.  d699fd33-5453-4a72-bb29-a0613caa16a1.

## User's Context

A user's context is a description of the projects, datasets, data series types and imagery types that are accessible to them. This description proivdes GUID identifiers that are used through the API to reference different information. 

## Integration API

The integration API is a standard Web Service RESTFul API. 

An application must sign into Imago and receive an authorisation token before further API calls are permitted. The authorisation token represents a session and expires after a set period of time.

Each API call must include the authorisation token using the header "imago-api-token" on the HTTP request.

## Light Table Linking

An application can also link to imagery stored in Imago and display it on the web site's light table.
