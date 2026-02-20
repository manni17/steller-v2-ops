# Integration V2 1

```csharp
IntegrationV21Controller integrationV21Controller = client.IntegrationV21Controller;
```

## Class Name

`IntegrationV21Controller`

## Methods

* [Brands](../../doc/controllers/integration-v2-1.md#brands)
* [Stock](../../doc/controllers/integration-v2-1.md#stock)
* [Categories](../../doc/controllers/integration-v2-1.md#categories)
* [Countries](../../doc/controllers/integration-v2-1.md#countries)
* [Place Order](../../doc/controllers/integration-v2-1.md#place-order)


# Brands

### GET /api/Integration/v2.0/brands

This endpoint retrieves a list of brands with optional filtering parameters.

#### Headers

- Content-Type: application/json

#### Request Parameters

- pagesize (optional): The number of items per page.

- pageIndex (optional): The index of the page.

- currencyCode (optional): Filter by currency code.

- countryCode (optional): Filter by country code.

- name (optional): Filter by brand name.

- sku (optional): Filter by stock keeping unit (SKU).

- categoryId (optional): Filter by category ID.

- modifiedDate (optional): Filter by modified date.

#### Response

The response returns a JSON object with the following fields:

- pageIndex: The index of the returned page.

- pageSize: The number of items per page.

- count: The total count of items.

- items: An array of brand objects with the following properties:
  
  - id: The unique identifier of the brand.
  
  - name: The name of the brand.
  
  - sku: The stock keeping unit (SKU) of the brand.
  
  - countryCode: The country code associated with the brand.
  
  - currencyCode: The currency code associated with the brand.
  
  - description: The description of the brand.
  
  - disclaimer: The disclaimer associated with the brand.
  
  - redemptionInstructions: Instructions for redemption.
  
  - terms: Terms and conditions for the brand.
  
  - status: The status of the brand.
  
  - logoUrl: The URL of the brand's logo.
  
  - modifiedDate: The date of the last modification.
  
  - faceValues: An array of face value objects with min and max values.
    
    - fixed face value > when min and max are equal =>
    
    - range face value > when Min < Max
    
    - steps also as 1 (integer)
    
    - one brand can have multiple face values range and fixed
  
  - categories: An array of category IDs associated with the brand.

Example Response:

```json
{
  "pageIndex": 0,
  "pageSize": 0,
  "count": 0,
  "items": [
    {
      "id": 0,
      "name": "",
      "sku": "",
      "countryCode": "",
      "currencyCode": "",
      "description": "",
      "disclaimer": null,
      "redemptionInstructions": "",
      "terms": "",
      "status": "",
      "logoUrl": "",
      "modifiedDate": "",
      "faceValues": [
        {
          "min": 1,
          "max": 500
        },
        {
          "min": 1000,
          "max": 1000
        },
        {
          "min": 2000,
          "max": 5000
        }
      ],
      "categories": [0]
    }
  ]
}

```

```csharp
BrandsAsync(
    int pagesize,
    int pageIndex,
    string currencyCode,
    string countryCode,
    string name,
    string sku,
    string categoryId,
    string modifiedDate)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `pagesize` | `int` | Query, Required | - |
| `pageIndex` | `int` | Query, Required | - |
| `currencyCode` | `string` | Query, Required | Iso 3 currency code |
| `countryCode` | `string` | Query, Required | Iso 2 country code |
| `name` | `string` | Query, Required | Contain in brand name |
| `sku` | `string` | Query, Required | brand Sku |
| `categoryId` | `string` | Query, Required | category Id |
| `modifiedDate` | `string` | Query, Required | Modified After |

## Response Type

[`Task<Models.Brands>`](../../doc/models/brands.md)

## Example Usage

```csharp
int pagesize = 10;
int pageIndex = 0;
string currencyCode = "currencyCode0";
string countryCode = "countryCode4";
string name = "name0";
string sku = "sku4";
string categoryId = "categoryId0";
string modifiedDate = "2023-10-02";
try
{
    Brands result = await integrationV21Controller.BrandsAsync(
        pagesize,
        pageIndex,
        currencyCode,
        countryCode,
        name,
        sku,
        categoryId,
        modifiedDate
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "pageIndex": 0,
  "pageSize": 10,
  "count": 2,
  "items": [
    {
      "id": 2118,
      "name": "Test Brand UAE",
      "sku": "TBRAND-AE",
      "countryCode": "AE",
      "currencyCode": "AED",
      "description": "test description for Brand UAE",
      "disclaimer": null,
      "redemptionInstructions": "Test redemption instructions for Brand UAE",
      "terms": "Test Terms and conditions for Brand UAE",
      "status": "Live",
      "logoUrl": "https://bamboo-assets.s3.amazonaws.com/app-images/brand-images/2118/card-picture",
      "modifiedDate": "2024-07-03T06:27:00.1331765",
      "faceValues": [
        {
          "min": 1,
          "max": 1
        },
        {
          "min": 10,
          "max": 10
        },
        {
          "min": 20,
          "max": 20
        }
      ],
      "categories": [
        6
      ]
    },
    {
      "id": 6132,
      "name": "Test Brand Bahrain",
      "sku": "TBRAND1-AE",
      "countryCode": "BH",
      "currencyCode": "BHD",
      "description": "Test description",
      "disclaimer": null,
      "redemptionInstructions": "Test redemption instructions",
      "terms": "Test terms and conditions ",
      "status": "TemporaryInactive",
      "logoUrl": "",
      "modifiedDate": "2024-10-16T16:29:48.3522299",
      "faceValues": [],
      "categories": []
    }
  ]
}
```


# Stock

### Get Stock Information

This endpoint retrieves the stock information for a specific item based on the provided SKU and face value.

#### Request Parameters

- `sku` (path) - The Stock Keeping Unit (SKU) of the item.

- `facevalue` (path) - The face value of the item.

#### Response

- `faceValue` (number) - The face value of the item.

- `quantity` (number) - The available quantity of the item.

- `isStocked` (boolean) - Indicates if the item is in stock. If the value is `false`, it indicates that the item is unlimited.

- `price` (number) - The price of the item.

- `currencyCode` (string) - The Iso-3 currency code for the price.

#### Example Response

```json
{
    "faceValue": 0,
    "quantity": 0,
    "isStocked": true,
    "price": 0,
    "currencyCode": ""
}

```

```json
{
    "type": "object",
    "properties": {
        "faceValue": {
            "type": "number"
        },
        "quantity": {
            "type": "number"
        },
        "isStocked": {
            "type": "boolean"
        },
        "price": {
            "type": "number"
        },
        "currencyCode": {
            "type": "string"
        }
    }
}

```

```csharp
StockAsync(
    string sku,
    int facevalue)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `sku` | `string` | Template, Required | unique Identifire |
| `facevalue` | `int` | Template, Required | requested face value |

## Response Type

[`Task<Models.Stock>`](../../doc/models/stock.md)

## Example Usage

```csharp
string sku = "TBRAND-AE";
int facevalue = 10;
try
{
    Stock result = await integrationV21Controller.StockAsync(
        sku,
        facevalue
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "faceValue": 10,
  "quantity": 2,
  "isStocked": true,
  "price": 20,
  "currencyCode": "BHD"
}
```


# Categories

This endpoint makes an HTTP GET request to retrieve a list of categories from the integration API. The response is in JSON format with a status code of 200.

### Response

The response will be a JSON array containing objects representing the categories. Each category object will have the following properties:

- `id` (number): The unique identifier for the category.

- `name` (string): The name of the category.

- `description` (string): The description of the category.

Example response:

```json
[
    {
        "id": 0,
        "name": "",
        "description": ""
    }
]

```

```json
{
  "type": "array",
  "items": {
    "type": "object",
    "properties": {
      "id": {
        "type": "number"
      },
      "name": {
        "type": "string"
      },
      "description": {
        "type": "string"
      }
    }
  }
}

```

```csharp
CategoriesAsync(
    string cacheControl)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `cacheControl` | `string` | Header, Required | - |

## Response Type

[`Task<List<Models.Category3>>`](../../doc/models/category-3.md)

## Example Usage

```csharp
string cacheControl = "public";
try
{
    List<Category3> result = await integrationV21Controller.CategoriesAsync(cacheControl);
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
[
  {
    "id": 1,
    "name": "Entertainment",
    "description": ""
  },
  {
    "id": 2,
    "name": "Gaming",
    "description": ""
  },
  {
    "id": 3,
    "name": "Prepaid Cards",
    "description": ""
  },
  {
    "id": 4,
    "name": "Fashion & Accessories",
    "description": ""
  },
  {
    "id": 5,
    "name": "Tools & Home Improvement",
    "description": ""
  },
  {
    "id": 6,
    "name": "Airlines & Hotels",
    "description": ""
  },
  {
    "id": 7,
    "name": "Beauty Wellness & Spa",
    "description": ""
  },
  {
    "id": 9,
    "name": "eCommerce",
    "description": ""
  },
  {
    "id": 10,
    "name": "Dining & Restaurants",
    "description": ""
  },
  {
    "id": 11,
    "name": "Kids Fashion & Toys",
    "description": ""
  },
  {
    "id": 12,
    "name": "Travel & Leisure",
    "description": ""
  },
  {
    "id": 13,
    "name": "Home & Garden",
    "description": ""
  },
  {
    "id": 14,
    "name": "Learning",
    "description": ""
  },
  {
    "id": 15,
    "name": "Sports & Lifestyle",
    "description": ""
  },
  {
    "id": 16,
    "name": "Electronics",
    "description": ""
  },
  {
    "id": 17,
    "name": "Crypto",
    "description": ""
  },
  {
    "id": 18,
    "name": "Supermarkets & Hypermarkets",
    "description": ""
  },
  {
    "id": 19,
    "name": "Bookstore",
    "description": ""
  },
  {
    "id": 20,
    "name": "Telecommunication & Internet",
    "description": ""
  },
  {
    "id": 21,
    "name": "Department Stores",
    "description": ""
  },
  {
    "id": 22,
    "name": "Mobile Application",
    "description": ""
  },
  {
    "id": 23,
    "name": "Shopping Mall",
    "description": ""
  },
  {
    "id": 24,
    "name": "Gas Station",
    "description": ""
  },
  {
    "id": 25,
    "name": "Drugstore",
    "description": ""
  },
  {
    "id": 26,
    "name": "Pet Shop & Pet Care",
    "description": ""
  },
  {
    "id": 27,
    "name": "Charity",
    "description": ""
  },
  {
    "id": 30,
    "name": "Petroleum",
    "description": ""
  }
]
```


# Countries

The endpoint retrieves a list of catalog countries through an HTTP GET request to {{host}}/api/integration/v2.0/catalog-countries.

The response returned is a JSON array with the following schema:

```json
[
  {
    "code": "",
    "name": "",
    "defaultCurrencyCode": "",
    "alpha3Code": ""
  }
]

```

This JSON schema represents the structure of the response where each object in the array contains the code, name, defaultCurrencyCode, and alpha3Code for a catalog country.

The response to the request will have a status code of 200 and a content type of application/json. The response body will contain an array of objects, where each object represents a catalog country. Each country object includes the following properties:

- code: The code of the country

- name: The name of the country

- defaultCurrencyCode: The default currency code of the country

- alpha3Code: The alpha-3 code of the country

```csharp
CountriesAsync()
```

## Response Type

`Task`

## Example Usage

```csharp
try
{
    await integrationV21Controller.CountriesAsync();
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```


# Place Order

### Checkout Order

This endpoint allows you to place an order by sending a POST request to the specified URL.

#### Request Body

The request body should be in JSON format and include the following parameters:

- `RequestId` (string) - A unique identifier for the request.

- `AccountId` (number) - The account ID associated with the order.

- `Products` (array) - An array of objects containing details about the products being ordered, including `Sku` (string), `Quantity` (number), and `Value` (number).

#### Response

Upon successful execution, the endpoint returns a JSON response with the following fields:

- `requestId` (string) - The unique identifier for the request.

- `orderNumber` (string) - The order number generated for the placed order.

- `status` (string) - The status of the order.

- `currencyCode` (string) - The currency code used for the transaction.

- `totalCards` (number) - The total number of cards associated with the order.

#### Example Request

```json
{
  "RequestId": "{{$guid}}",
  "AccountId": 307,
  "Products": [
    {
      "Sku": "TBRAND-AE",
      "Quantity": 1,
      "Value": 10
    }
  ]
}

```

#### Example Response

```json
{
  "requestId": "",
  "orderNumber": "",
  "status": "",
  "currencyCode": "",
  "totalCards": 0
}

```

```csharp
PlaceOrderAsync(
    string cacheControl,
    Models.PlaceOrderRequest1 body)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `cacheControl` | `string` | Header, Required | - |
| `body` | [`PlaceOrderRequest1`](../../doc/models/place-order-request-1.md) | Body, Required | - |

## Response Type

[`Task<Models.PlaceOrder>`](../../doc/models/place-order.md)

## Example Usage

```csharp
string cacheControl = "public";
PlaceOrderRequest1 body = new PlaceOrderRequest1
{
    RequestId = "{{$guid}}",
    AccountId = 307,
    Products = new List<Product3>
    {
        new Product3
        {
            Sku = "TBRAND-AE",
            Quantity = 1,
            MValue = 10,
        },
    },
};

try
{
    PlaceOrder result = await integrationV21Controller.PlaceOrderAsync(
        cacheControl,
        body
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "requestId": "efb96235-5ec7-4969-852b-ccea48c8d906",
  "orderNumber": "O-10531861",
  "status": "Created",
  "currencyCode": "USD",
  "totalCards": 1
}
```

