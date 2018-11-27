#
# Download Image from Imago
#
# Imago Inc
# November, 2018
#

import requests as req
from PIL import Image
import io 


apiEndpoint = 'https://io.imago.live/integrate/2'

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

workspaces = res.json()["workspaces"]

print("The available workspaces and datasets are:\n")
for workspace in workspaces:
    print(workspace["name"])
    for dataset in workspace["datasets"]:
        print("\t", dataset["name"])

#
# Find the Imago Demo workspace
#

workspace = firstOrNone([p for p in workspaces if p["name"] == "Imago Demo"])
if workspace is None:
    print('Unable to find the workspace Imago Demo.')
    exit()

print("Found workspace: ", workspace["name"])

# 
# Find the Drilling Hard Rock dataset
#

dataset = firstOrNone([d for d in workspace["datasets"] if d["name"] == "Drilling Hard Rock"])
if dataset is None:
    print('Unable to find the dataset Drilling Hard Rock.')
    exit()

print("Found dataset: ", dataset["name"])
 
#
# Find a collection called AA222 in Drilling Hard Rock
#

res = req.get(apiEndpoint + "/collection", params={ "name" : "AA222", "datasetid" : dataset["id"] }, headers=apiHeaders)
if res.status_code != 200:
    print('Unable to find the collection called AA222.')
    exit()

collection = firstOrNone(res.json()["collections"])
if collection is None:
    print('Unable to find the collection called AA222.')
    exit()
    
print("Found collection: ", collection["name"])

#
# Find Core Boxes data series type
#

imageryType = firstOrNone([d for d in dataset["imageryTypes"] if d["name"] == "Core Boxes"])
if imageryType is None:
    print('Unable to find the data series type Core Boxes in the Drilling Hard Rock dataset.')
    exit()

print("Found imagery type: ", imageryType["name"])

#
# Find imagery for the interval 0-4.6 in the data series Core Boxes for data entity AA222
#

res = req.get(apiEndpoint + "/imagery", params={ "collectionid" : collection["id"], "imagerytypeid" : imageryType["id"], "startdepth" : "0.0", "enddepth" : "4.6" }, headers=apiHeaders)
if res.status_code != 200:
    print('Unable to find the data item for interval 0-4.6.')
    exit()

imagery = firstOrNone(res.json()["imageries"])
if imagery is None:
    print('Unable to find the imagery for interval 0-4.6.')
    exit()
    
print("Found imagery: ", imagery["startDepth"], imagery["endDepth"])

#
# Find the Dry image type in the imagery at 0-4.6
#

imageType = firstOrNone([d for d in imageryType["imageTypes"] if d["name"] == "Dry"])
if imageType is None:
    print('Unable to find the image type Dry.')
    exit()

print("Found image type ", imageType["name"])

#
# Download an image of the core tray
#

print("Downloading image ...")
res = req.get(apiEndpoint + "/image", params={ "imageryid" : imagery["id"], "imagetypeid" : imageType["id"] }, headers=apiHeaders)
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