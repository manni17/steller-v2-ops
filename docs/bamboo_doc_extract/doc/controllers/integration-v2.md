# Integration V2

```csharp
IntegrationV2Controller integrationV2Controller = client.IntegrationV2Controller;
```

## Class Name

`IntegrationV2Controller`


# Get Catalog

Users have to include valid credentials in the Authorization header of each GET request.

The GET /api/integration/v2.0/catalog endpoint lets you retrieve the list of brands and products available for you.

A successful GET request returns a list of available brands in the response body with the information for each brand.

### Example response:

```json
{
    "pageindex": 0,
    "pageSize": 100,
    "count": 1,
    "items": [
        {
            "name": "iTunes USA eGift voucher",
            "countryCode": "US",
            "currencyCode": "USD",
            "description": "string",
            "disclaimer": "string",
            "redemptionInstructions": "string",
            "terms": "string",
            "logoUrl": "string",
            "modifiedDate": "2022-08-22T09:06:28.4131483",
            "products": [
                {
                    "id": 114111,
                    "name": "iTunes USA eGift voucher",
                    "minFaceValue": 100.0000,
                    "maxFaceValue": 100.0000,
                    "count": null,
                    "price": {
                        "min": 100.00000000,
                        "max": 100.00000000,
                        "currencyCode": "USD"
                    },
                    "modifiedDate": "2022-08-22T09:06:29.6783345"
                }
            ]
        }
    ]
}

```

```csharp
GetCatalogAsync(
    string currencyCode,
    string countryCode,
    string name,
    string modifiedDate,
    int productId,
    int pageSize,
    int pageIndex,
    int brandId,
    string targetCurrency)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `currencyCode` | `string` | Query, Required | filter by passing the 3 chars Iso standard currncy format |
| `countryCode` | `string` | Query, Required | filter by passing the 2 chars Iso standard country format |
| `name` | `string` | Query, Required | filter by brand name |
| `modifiedDate` | `string` | Query, Required | filter product modified after spesified date |
| `productId` | `int` | Query, Required | filter only the product with that Id if exist |
| `pageSize` | `int` | Query, Required | page size default: 100 |
| `pageIndex` | `int` | Query, Required | page index |
| `brandId` | `int` | Query, Required | filter all product with this brand Id |
| `targetCurrency` | `string` | Query, Required | display all prices in single currency |

## Response Type

[`Task<Models.GetCatalog1>`](../../doc/models/get-catalog-1.md)

## Example Usage

```csharp
string currencyCode = "USD";
string countryCode = "US";
string name = "playstation germany";
string modifiedDate = "2022-08-21";
int productId = 114111;
int pageSize = 100;
int pageIndex = 0;
int brandId = 1020;
string targetCurrency = "USD";
try
{
    GetCatalog1 result = await integrationV2Controller.GetCatalogAsync(
        currencyCode,
        countryCode,
        name,
        modifiedDate,
        productId,
        pageSize,
        pageIndex,
        brandId,
        targetCurrency
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

