define({ "api": [
  {
    "type": "get",
    "url": "/integrate/2/context",
    "title": "",
    "name": "GetUserContext",
    "group": "User_Context",
    "description": "<p>The user context describes the list of projects accessible by that user.</p> <p>Each project is described by its dataset, data series types and imagery type definitions.</p>",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "optional": false,
            "field": "imago-api-token",
            "description": ""
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
      }
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "ErrorDuringRequest",
            "description": "<p>Internal error occurred during the request</p>"
          }
        ]
      }
    },
    "version": "2.0.0",
    "filename": "../../imago-agent/api/routes/io/2/get-context.js",
    "groupTitle": "User_Context"
  }
] });
