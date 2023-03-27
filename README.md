# ConversionMicroservice 
The **ConversionMicroservice** is part of a university project involving Dataspaces. This API is designed to convert data from JsonLD format to other formats, such as JSON to CSV or XML.

## Technologies
The ConversionMicroservice API is built using the following technologies:

* ASP.NET Core 
* C#

## Endpoints
The ConversionMicroservice API has only one endpoint:

### /get_data
This endpoint is used to retrieve data from the API in the specified format. The endpoint accepts the following query parameters:

* apiUrl: The URL of the API to retrieve data from.
* type: The format in which the data should be returned. Possible values are "json", "csv", or "xml".

## Example Usage
```
http://localhost:PORT/get_data?apiUrl=http://example.com/data&type=json
```
## Controller
The **ConversionController** is responsible for handling incoming HTTP requests and returning responses. It uses the **ConversionProcessor** to perform the data conversions.

### Methods
**GetAll**: This method retrieves data from the specified API and returns it in the specified format.

## Processor
The **ConversionProcessor** is responsible for converting data from one format to another.

## Methods
* **GetAll**: This method retrieves data from the specified API and converts it to the specified format.
* **ConvertJsonLdToJson**: This method converts JSON-LD data to regular JSON format.
* **ConvertJsonToXml**: This method converts JSON data to XML format.
* **ConvertJsonToCsv**: This method converts JSON data to CSV format.
