# Building the Project

go to the Project Directory

run 'dotnet build dotnet build .\CsvApp.Service.csproj'

# Running the Project

## From Explorer
From the Bin Directory on a Windows machine run the CsvApp.Service.exe

## From dotnet tools
in the Bin Directory run dotnet .\CsvApp.Service.dll

## Swagger Documentation
from a browser on https://localhost:5000/swagger/index.html the swagger documentation can be found.

# Testing from Postman

Import the Swagger.postman_collection.json into Postman and test accordingly


# Testing from Curl

## GET Vechicle
curl --location 'https://localhost:5000/api/Vehicle'

## GET Vechicle
curl --location 'https://localhost:5000/api/Vehicle/{id}'

## Add Vechicle

curl --location 'https://localhost:7098/api/Vehicle' \
--header 'Content-Type: application/json' \
--data '{
  "type": "string",
  "make": "string",
  "model": "string",
  "year": 0,
  "wheelCount": 0,
  "fuelType": "string",
  "active": true
}'

## Update Vechicle

curl --location --request PUT 'https://localhost:7098/api/Vehicle' \
--header 'Content-Type: application/json' \
--data '{
        "id": "{Vehcicle ID}",
        "type": "string",
        "make": "string",
        "model": "string",
        "year": 0,
        "wheelCount": 0,
        "fuelType": "string",
        "active": true
    }'


## Delete Vechicle
curl --location --request DELETE 'https://localhost:7098/api/Vehicle/{id}}'

## Query Vehcicle
curl --location 'https://localhost:7098/api/Vehicle/query?make={make}&type={type}'