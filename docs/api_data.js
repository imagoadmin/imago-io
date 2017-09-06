define({ "api": [
  {
    "type": "get",
    "url": "/integrate/1/imagery",
    "title": "Get data attributes",
    "name": "GetAttributes",
    "group": "Attributes",
    "description": "<p>Return a group of attributes associated with data within Imago.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -O  http://io.imago.live/integrate/1/attributes?id=e1865861-0d53-4d49-a554-af79dae9aa81&type=image",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "id",
            "description": "<p>Data identifier.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "allowedValues": [
              "\"image\""
            ],
            "optional": true,
            "field": "type",
            "defaultValue": "image",
            "description": "<p>Type of data to search for.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "allowedValues": [
              "\"properties\""
            ],
            "optional": true,
            "field": "group",
            "defaultValue": "properties",
            "description": "<p>Specifies which group of attributes to return for the data.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Query String:",
          "content": "?id=e1865861-0d53-4d49-a554-af79dae9aa81&type=image",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "id",
            "description": "<p>Data identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "group",
            "description": "<p>Name of group of attributes.</p>"
          },
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "attributes",
            "description": "<p>Attributes associated with the specified group. The fields depend on the attributes group and its definition.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\" : \"108501d0-4d6c-475c-8ad2-96bb983b1fc5\",\n    \"group\" : \"properties\",\n    \"attributes\" : {\n        \"mimeType\" : \"image/jpeg\",\n        \"fileName\" : \"DH001_coretray_wet.jpeg\",\n        \"fileExt\" : \".jpeg\",\n        \"fileSize\" : \"16253742783\"\n    }\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The specified attributes were not found.</p>"
          },
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/get-attributes.js",
    "groupTitle": "Attributes"
  },
  {
    "type": "get",
    "url": "/integrate/1/dataentity",
    "title": "Search for data entities",
    "name": "GetDataEntity",
    "group": "DataEntity",
    "description": "<p>Searches for a list of matching data entities.</p> <p>Each project is described by its dataset, data series type and imagery type definitions.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -O  http://io.imago.live/integrate/1/imagery?datasetid=9a08e64f-e6e9-41d8-a47c-044db8a882c4&name=DH&match=startswidth",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": true,
            "field": "name",
            "description": "<p>Search pattern to match data entity names. If no search pattern is supplied then all data entities are returned for the dataset.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "allowedValues": [
              "\"equals\"",
              "\"startswith\"",
              "\"endswith\"",
              "\"includes\""
            ],
            "optional": true,
            "field": "match",
            "defaultValue": "equals",
            "description": "<p>Determines how the search pattern matches the data entity name.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": true,
            "field": "datasetid",
            "description": "<p>Only search within the dataset.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Query String:",
          "content": "?datasetid=9a08e64f-e6e9-41d8-a47c-044db8a882c4&name=DH&match=startswidth",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "dataEntities",
            "description": "<p>List of matching data entities.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataEntities.id",
            "description": "<p>Data entity identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "dataEntities.name",
            "description": "<p>Name of data entity.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataEntities.datasetId",
            "description": "<p>Identifier of dataset that the data entity belongs to.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"dataEntities\" : [\n        {\n            \"id\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n            \"name\" : \"DH001\",\n            \"datasetId\" : \"9a08e64f-e6e9-41d8-a47c-044db8a882c4\"\n        },\n        {\n            \"id\" : \"eecd8f6c-127f-42ba-9ddc-a56a97c61719\",\n            \"name\" : \"DH003\",\n            \"datasetId\" : \"9a08e64f-e6e9-41d8-a47c-044db8a882c4\"\n        }\n    ]\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/get-dataentity.js",
    "groupTitle": "DataEntity"
  },
  {
    "type": "post",
    "url": "/integrate/1/dataentity",
    "title": "Create a new data entity",
    "name": "PostDataEntity",
    "group": "DataEntity",
    "description": "<p>Adds a new data entity to a dataset.</p> <p>If a data entity already exists in the dataset then no action is taken.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -H \"Content-Type: application/json\" -X POST -d '{\"name\":\"DH001\",\"datasetId\":\"9a08e64f-e6e9-41d8-a47c-044db8a882c4\"}' http://io.imago.live/integrate/1/dataentity",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "name",
            "description": "<p>Name of new data entity.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "datasetId",
            "description": "<p>Add the new data entity to this dataset.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Request Body:",
          "content": "{\n    \"name\" : \"DH001\",\n    \"datasetId\" : \"9a08e64f-e6e9-41d8-a47c-044db8a882c4\"\n}",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "id",
            "description": "<p>New data entity identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "name",
            "description": "<p>Name of new data entity.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "datasetId",
            "description": "<p>Identifier of dataset that the data entity belongs to.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n    \"name\" : \"DH001\",\n    \"datasetId\" : \"9a08e64f-e6e9-41d8-a47c-044db8a882c4\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/post-dataentity.js",
    "groupTitle": "DataEntity"
  },
  {
    "type": "get",
    "url": "/integrate/1/dataitem",
    "title": "Search for data items",
    "name": "GetDataItem",
    "group": "DataItem",
    "description": "<p>Searches for a list of matching data items.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -O  http://io.imago.live/integrate/1/imagery?dataentityid=d856d001-22bf-4339-8382-9e29532e539b&dataseriestypeid=78742fab-4c55-4b57-830d-5ab6b6c1fb09",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataentityid",
            "description": "<p>Only search within this data entity.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataseriestypeid",
            "description": "<p>Only search for data items with this data series type.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": true,
            "field": "name",
            "description": "<p>Search for data items with this name.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "startdepth",
            "description": "<p>Search for data items with this start depth.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "enddepth",
            "description": "<p>Search for data items with this end depth.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "x",
            "description": "<p>Search for data items with this longtitude/X coordinate.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "y",
            "description": "<p>Search for data items with this latitude/Y coordinate.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "z",
            "description": "<p>Search for data items with this elevation/Z coordinate.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "tol",
            "defaultValue": "0.0",
            "description": "<p>Tolerance used when matching numbers.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Query String:",
          "content": "?dataentityid=d856d001-22bf-4339-8382-9e29532e539b&dataseriestypeid=78742fab-4c55-4b57-830d-5ab6b6c1fb09",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "dataItems",
            "description": "<p>List of matching data items.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataItems.id",
            "description": "<p>Data item identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataItems.dataEntityId",
            "description": "<p>Identifier of data entity that the data item belongs to.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataItems.dataSeriesTypeId",
            "description": "<p>Identifier of data series type that the data item belongs to within the data entity.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": true,
            "field": "dataItems.name",
            "description": "<p>Name of the data item</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "dataItems.startDepth",
            "description": "<p>Start depth of the data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "dataItems.endDepth",
            "description": "<p>End depth of the data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "dataItems.x",
            "description": "<p>Longtitude/X coordinate of the data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "dataItems.y",
            "description": "<p>Latitude/Y coordinate of the data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "dataItems.z",
            "description": "<p>Elevation/Z coordinate of the data item.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"dataItems\" : [\n        {\n            \"id\" : \"c2019bd6-34aa-4561-ab2f-8802fa5ff3a9\",\n            \"dataEntityId\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n            \"dataSeriesTypeId\" : \"78742fab-4c55-4b57-830d-5ab6b6c1fb09\"\n            \"startDepth\" : 0,\n            \"endDepth\" : 10.1,\n        },\n        {\n            \"id\" : \"5a199953-52e5-437c-a2d8-aa8785beb45d\",\n            \"dataEntityId\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n            \"dataSeriesTypeId\" : \"78742fab-4c55-4b57-830d-5ab6b6c1fb09\",\n            \"startDepth\" : 10.1,\n            \"endDepth\" : 15.2,\n        },\n        {\n            \"id\" : \"ca8379b2-261e-4f30-8848-2eb92cfded87\",\n            \"dataEntityId\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n            \"dataSeriesTypeId\" : \"78742fab-4c55-4b57-830d-5ab6b6c1fb09\"\n            \"startDepth\" : 15.2,\n            \"endDepth\" : 19.43,\n        }\n    ]\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/get-dataitem.js",
    "groupTitle": "DataItem"
  },
  {
    "type": "post",
    "url": "/integrate/1/dataitem",
    "title": "Add a data item to a data entity",
    "name": "PostDataItem",
    "group": "DataItem",
    "description": "<p>Add a new data item of the specified data series type to an existing data entity.</p> <p>If a data item exists with the same combination of name/start depth/end depth/X/Y/Z then no action is taken.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -H \"Content-Type: application/json\" -X POST -d '{\"dataEntityId\":\"DH001\",\"dataSeriesTypeId\":\"9a08e64f-e6e9-41d8-a47c-044db8a882c4\",\"name\":\"Sample 01926AB\"}' http://io.imago.live/integrate/1/dataitem",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataEntityId",
            "description": "<p>Add to this data entity.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataSeriesTypeId",
            "description": "<p>Add a data item of this data series type.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": true,
            "field": "name",
            "description": "<p>Name of new data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "startDepth",
            "description": "<p>Start depth of new data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "endDepth",
            "description": "<p>End depth of new data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "x",
            "description": "<p>Longtitude/X coordinate of new data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "y",
            "description": "<p>Latitude/Y coordinate of new data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "z",
            "description": "<p>Elevation/Z coordinate of new data item.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Request Body:",
          "content": "{\n    \"dataEntityId\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n    \"dataSeriesTypeId\" : \"78742fab-4c55-4b57-830d-5ab6b6c1fb09\"\n    \"name\" : \"DH001\",\n    \"startDepth\" : 0,\n    \"endDepth\" : 10.1\n}",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "id",
            "description": "<p>New data item identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataEntityId",
            "description": "<p>Identifier of data entity that the new data item belongs to.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "dataSeriesTypeId",
            "description": "<p>Identifier of data series type that the new data item belongs to within the data entity.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": true,
            "field": "name",
            "description": "<p>Name of the new data item</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "startDepth",
            "description": "<p>Start depth of the new data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "endDepth",
            "description": "<p>End depth of the new data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "x",
            "description": "<p>Longtitude/X coordinate of the new data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "y",
            "description": "<p>Latitude/Y coordinate of the new data item.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": true,
            "field": "z",
            "description": "<p>Elevation/Z coordinate of the new data item.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\" : \"c2019bd6-34aa-4561-ab2f-8802fa5ff3a9\",\n    \"dataEntityId\" : \"d856d001-22bf-4339-8382-9e29532e539b\",\n    \"dataSeriesTypeId\" : \"78742fab-4c55-4b57-830d-5ab6b6c1fb09\"\n    \"startDepth\" : 0,\n    \"endDepth\" : 10.1\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/post-dataitem.js",
    "groupTitle": "DataItem"
  },
  {
    "type": "get",
    "url": "/integrate/1/imagery",
    "title": "Download an image",
    "name": "GetImagery",
    "group": "Imagery",
    "description": "<p>Downloads an image</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -O  http://io.imago.live/integrate/1/imagery?dataitemid=c2019bd6-34aa-4561-ab2f-8802fa5ff3a9&imagerytypeid=f0b6aec1-ce5c-4874-b107-162090623a9b&mimetype=image%2Fjpeg&width=200",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataitemid",
            "description": "<p>Only search within this data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "imagerytypeid",
            "description": "<p>Only search for an image with this imagery type.</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "width",
            "description": "<p>Scale the image to this width (height is adjusted according to aspect ratio if not specified).</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": true,
            "field": "height",
            "description": "<p>Scale the image to this height (width is adjusted according to aspect ratio if not specified).</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Query String: ",
          "content": "?dataitemid=c2019bd6-34aa-4561-ab2f-8802fa5ff3a9&imagerytypeid=f0b6aec1-ce5c-4874-b107-162090623a9b&mimetype=image%2Fjpeg&width=200",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The specified imagery was not found.</p>"
          },
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/get-imagery.js",
    "groupTitle": "Imagery"
  },
  {
    "type": "post",
    "url": "/integrate/1/imagery",
    "title": "Upload an image",
    "name": "PostImagery",
    "group": "Imagery",
    "description": "<p>Updloads an image of a specified imagery type to a data item.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -i -F filedata=@sample.jpeg  http://io.imago.live/integrate/1/imagery?dataitemid=c2019bd6-34aa-4561-ab2f-8802fa5ff3a9&imagerytypeid=f0b6aec1-ce5c-4874-b107-162090623a9b&mimetype=image%2Fjpeg&history=replace",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "dataitemid",
            "description": "<p>Add to this data item.</p>"
          },
          {
            "group": "Parameter",
            "type": "Guid",
            "optional": false,
            "field": "imagerytypeid",
            "description": "<p>Add an image of this imagery type.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "allowedValues": [
              "\"image/png\"",
              "\"image/jpeg\""
            ],
            "optional": false,
            "field": "mimetype",
            "description": "<p>Image's mime type</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "allowedValues": [
              "\"replace\"",
              "\"append\""
            ],
            "optional": true,
            "field": "history",
            "defaultValue": "replace",
            "description": "<p>Defines what do to if an image of the same imagery type already exists in the data item. By default &quot;replace&quot;, all previous images of the same imagery type are replaced. If &quot;append&quot; is specified then the image is added to the imagery type's history on the data item and made the most current image.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Query String:",
          "content": "?dataitemid=c2019bd6-34aa-4561-ab2f-8802fa5ff3a9&imagerytypeid=f0b6aec1-ce5c-4874-b107-162090623a9b&mimetype=image%2Fjpeg&history=replace",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "id",
            "description": "<p>New image's identifier.</p>"
          }
        ]
      }
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/post-imagery.js",
    "groupTitle": "Imagery"
  },
  {
    "type": "delete",
    "url": "/integrate/1/session",
    "title": "Sign out of Imago",
    "name": "DeleteSession",
    "group": "Session",
    "description": "<p>Sign a user out of imago and ends the current active session.</p>",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "error": {
      "fields": {
        "401": [
          {
            "group": "401",
            "optional": false,
            "field": "NotAuthorised",
            "description": "<p>Not authorised to sign in with the supplied credentials.</p>"
          }
        ],
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/definition.js",
    "groupTitle": "Session"
  },
  {
    "type": "get",
    "url": "/integrate/1/session",
    "title": "Check if still signed in",
    "name": "GetSession",
    "group": "Session",
    "description": "<p>Checks if a user is still signed into imago and has an active session.</p>",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "uid",
            "description": "<p>User's identifier.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"uid\" : \"23c6727c-a5a6-484b-ac33-9812b9878f0a\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "401": [
          {
            "group": "401",
            "optional": false,
            "field": "NotAuthorised",
            "description": "<p>Not authorised to sign in with the supplied credentials.</p>"
          }
        ],
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/definition.js",
    "groupTitle": "Session"
  },
  {
    "type": "put",
    "url": "/integrate/1/session",
    "title": "Sign into Imago",
    "name": "PutSession",
    "group": "Session",
    "description": "<p>Signs into Imago using a username and password and creates an active session.</p>",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "username",
            "description": "<p>User's account name.</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "password",
            "description": "<p>User's password.</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "uid",
            "description": "<p>User's identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "apiToken",
            "description": "<p>Imago authorisation token.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"uid\" : \"23c6727c-a5a6-484b-ac33-9812b9878f0a\",\n    \"apiToken\" : \"b4ecb7d7-b8bb-460f-9506-134df358f471\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "401": [
          {
            "group": "401",
            "optional": false,
            "field": "NotAuthorised",
            "description": "<p>Not authorised to sign in with the supplied credentials.</p>"
          }
        ],
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request.</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/definition.js",
    "groupTitle": "Session"
  },
  {
    "type": "get",
    "url": "/integrate/1/context",
    "title": "Get the user's context",
    "name": "GetUserContext",
    "group": "UserContext",
    "description": "<p>The user context describes the list of projects and other information accessible by that user.</p> <p>Each project is described by its dataset, data series type and imagery type definitions.</p>",
    "examples": [
      {
        "title": "Example Usage (requires the authorisation token header):",
        "content": "curl -O  http://io.imago.live/integrate/1/context",
        "type": "curl"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": "<p>b4ecb7d7-b8bb-460f-9506-134df358f471</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "projects",
            "description": "<p>List of projects accessible by the signed in user.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "projects.id",
            "description": "<p>Project identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.name",
            "description": "<p>Name of the project.</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "projects.datasets",
            "description": "<p>List of datasets in the project.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "projects.datasets.id",
            "description": "<p>Dataset identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.datasets.name",
            "description": "<p>Name of the dataset.</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes",
            "description": "<p>List of data series types in the dataset.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.id",
            "description": "<p>Data series type identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.name",
            "description": "<p>Name of the data series type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.contentType",
            "description": "<p>Content type of the data series type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.geometryType",
            "description": "<p>Geometry type of the data series type.</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.imagerytypes",
            "description": "<p>List of imagery types in the data series type.</p>"
          },
          {
            "group": "Success 200",
            "type": "Guid",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.imageryTypes.id",
            "description": "<p>Imagery type identifier.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "projects.datasets.dataSeriesTypes.imageryTypes.name",
            "description": "<p>Name of the imagery type.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"projects\": [\n        {\n            \"id\": \"e0a14890-4ff9-4048-bf5d-76018b950317\",\n            \"name\": \"Demo\",\n            \"datasets\": [\n                {\n                    \"id\": \"9a08e64f-e6e9-41d8-a47c-044db8a882c4\",\n                    \"name\": \"Base Metals Brownfields Drilling\",\n                    \"dataSeriesTypes\": [\n                        {\n                            \"id\": \"b3627180-ca97-4e5d-a0c6-49fbc0c16f6e\",\n                            \"name\": \"Core Trays\",\n                            \"geometryType\": \"trace\",\n                            \"contentType\": \"coretray\",\n                            \"imageryTypes\": [\n                                {\n                                    \"id\": \"744b14c5-f754-4dee-b644-bbfde533c798\",\n                                    \"name\": \"Dry\"\n                                },\n                                {\n                                    \"id\": \"d0969932-d6bd-4e0d-8c54-9acc13cf5144\",\n                                    \"name\": \"Wet\"\n                                }\n                            ]\n                        },\n                        {\n                            \"id\": \"aed9256f-a18a-4eb4-a17a-9e5958dddf2f\",\n                            \"name\": \"Downhole\",\n                            \"geometryType\": \"trace\",\n                            \"contentType\": \"none\",\n                            \"imageryTypes\": [\n                                {\n                                    \"id\": \"7bf27c47-7c0d-498e-a3fb-887531632b7b\",\n                                    \"name\": \"Dry\"\n                                },\n                                {\n                                    \"id\": \"02bcc28c-b483-422f-8d32-dac72285c3ea\",\n                                    \"name\": \"Wet\"\n                                }\n                            ]\n                        }\n                    ]\n                },\n                {\n                    \"id\": \"992efe3e-340c-4082-a83f-ddff9225c180\",\n                    \"name\": \"Coal Mine Infill Drilling\",\n                    \"dataSeriesTypes\": [\n                        {\n                            \"id\": \"b3627180-ca97-4e5d-a0c6-49fbc0c16f6e\",\n                            \"name\": \"Core Trays\",\n                            \"geometryType\": \"trace\",\n                            \"contentType\": \"coretray\",\n                            \"imageryTypes\": [\n                                {\n                                    \"id\": \"744b14c5-f754-4dee-b644-bbfde533c798\",\n                                    \"name\": \"Dry\"\n                                },\n                                {\n                                    \"id\": \"d0969932-d6bd-4e0d-8c54-9acc13cf5144\",\n                                    \"name\": \"Wet\"\n                                }\n                            ]\n                        },\n                        {\n                            \"id\": \"aed9256f-a18a-4eb4-a17a-9e5958dddf2f\",\n                            \"name\": \"Downhole\",\n                            \"geometryType\": \"trace\",\n                            \"contentType\": \"none\",\n                            \"imageryTypes\": [\n                                {\n                                    \"id\": \"7bf27c47-7c0d-498e-a3fb-887531632b7b\",\n                                    \"name\": \"Dry\"\n                                },\n                                {\n                                    \"id\": \"02bcc28c-b483-422f-8d32-dac72285c3ea\",\n                                    \"name\": \"Wet\"\n                                }\n                            ]\n                        }\n                    ]\n                }\n            ]\n        }\n    ]\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "404": [
          {
            "group": "404",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "1.0.0",
    "filename": "../imago-agent/api/routes/io/2/get-context.js",
    "groupTitle": "UserContext"
  }
] });
