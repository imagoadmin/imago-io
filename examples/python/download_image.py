#
# Download Image from Imago
#
# Imago Inc
# September, 2017
#

import requests as req
from PIL import Image
import io 


#apiEndpoint = 'https://io.imago.live/integrate/1'
apiEndpoint = 'http://localhost:3000/integrate/1'

def firstOrNone(l):
    if l is None or len(l) != 1:
        return None
    return l[0]

fileExtensions = { "image/jpeg":".jpg","image/png":".png" }

#
# Connect to Imago and get a session api token
#

res = req.put(apiEndpoint + '/session', data = { "username" : "imagolive", "password" : "123" })
if res.status_code != 200:
    print('Unable to connect to Imago.')
    exit()

apiToken = res.json()["apiToken"]

print('API token for this session is ', apiToken)
apiHeaders = {"imago-api-token" : apiToken}

#
# Get a list of the person's projects and datasets
#

res = req.get(apiEndpoint + "/context", headers=apiHeaders)
if res.status_code != 200:
    print('Unable to get the list of projects and datasets for this demo.')
    exit()

projects = res.json()["projects"]

print("The available projects and datasets are:\n")
for project in projects:
    print(project["name"])
    for dataset in project["datasets"]:
        print("\t", dataset["name"])

#
# Find the Imago Demo project
#

project = firstOrNone([p for p in projects if p["name"] == "Imago Demo"])
if project is None:
    print('Unable to find the project Imago Demo.')
    exit()

print("Found project: ", project["name"])

# 
# Find the Drilling Hard Rock dataset
#

dataset = firstOrNone([d for d in project["datasets"] if d["name"] == "Drilling Hard Rock"])
if dataset is None:
    print('Unable to find the dataset Drilling Hard Rock.')
    exit()

print("Found dataset: ", dataset["name"])
 
#
# Find a data entity called AA222 in Drilling Hard Rock
#

res = req.get(apiEndpoint + "/dataentity", params={ "name" : "AA222", "datasetid" : dataset["id"] }, headers=apiHeaders)
if res.status_code != 200:
    print('Unable to find the data entity called AA222.')
    exit()

dataEntity = firstOrNone(res.json()["dataEntities"])
if dataEntity is None:
    print('Unable to find the data entity called AA222.')
    exit()
    
print("Found data entity: ", dataEntity["name"])

#
# Find Core Boxes data series type
#

dataSeriesType = firstOrNone([d for d in dataset["dataSeriesTypes"] if d["name"] == "Core Boxes"])
if dataSeriesType is None:
    print('Unable to find the data series type Core Boxes in the Drilling Hard Rock dataset.')
    exit()

print("Found dataSeriesType: ", dataSeriesType["name"])

#
# Find data item for the interval 0-4.6 in the data series Core Boxes for data entity AA222
#

res = req.get(apiEndpoint + "/dataitem", params={ "dataentityid" : dataEntity["id"], "dataseriestypeid" : dataSeriesType["id"], "startdepth" : "0.0", "enddepth" : "4.6" }, headers=apiHeaders)
if res.status_code != 200:
    print('Unable to find the data item for interval 0-4.6.')
    exit()

dataItem = firstOrNone(res.json()["dataItems"])
if dataItem is None:
    print('Unable to find the data item for interval 0-4.6.')
    exit()
    
print("Found data item: ", dataItem["startDepth"], dataItem["endDepth"])

#
# Find the Dry imagery type in the data item at 0-4.6
#

imageryType = firstOrNone([d for d in dataSeriesType["imageryTypes"] if d["name"] == "Dry"])
if imageryType is None:
    print('Unable to find the imagery type Dry.')
    exit()

print("Found imagery type ", imageryType["name"])

#
# Download an image of the core tray
#

print("Downloading image ...")
res = req.get(apiEndpoint + "/imagery", params={ "dataitemid" : dataItem["id"], "imagerytypeid" : imageryType["id"] }, headers=apiHeaders)
if res.status_code != 200:
    print('Unable to find the download the core tray image.')
    exit()

# Construct the file name using the mime type
mimeType = res.headers["Content-Type"]
image_filename = "coretray" + fileExtensions[mimeType]

# Save the image to coretray.<mime type>
with io.BytesIO(res.content) as b:
    img = Image.open(b)
    img.save(image_filename)

print("Saved image to ", image_filename)