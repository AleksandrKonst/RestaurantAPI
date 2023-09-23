# RestaurantAPI
### Hypertext Application Language
```json
{
  "_links": {
    "self": {
      "href": "/api/clients"
    },
    "next": {
      "href": "/api/clients?index=5"
    },
    "final": {
      "href": "/api/clients?index=5&count=5"
    }
  },
  "count": 5,
  "total": 6,
  "index": 0,
  "items": [
    {
      "Code": "CA01",
      "Name": "Valera",
      "Number": "8-888-888-88-88",
      "_links": {
        "self": {
          "href": "/api/clients/CA01"
        },
        "order": {
          "href": "/api/orders/client/CA01"
        }
      }
    },
    {
      "Code": "CG02",
      "Name": "Vlad",
      "Number": "8-888-888-88-88",
      "_links": {
        "self": {
          "href": "/api/clients/CG02"
        },
        "order": {
          "href": "/api/orders/client/CG02"
        }
      }
    },
    {
      "Code": "CD03",
      "Name": "Roman",
      "Number": "8-888-888-88-88",
      "_links": {
        "self": {
          "href": "/api/clients/CD03"
        },
        "order": {
          "href": "/api/orders/client/CD03"
        }
      }
    },
    {
      "Code": "CC04",
      "Name": "Bob",
      "Number": "8-888-888-88-88",
      "_links": {
        "self": {
          "href": "/api/clients/CC04"
        },
        "order": {
          "href": "/api/orders/client/CC04"
        }
      }
    },
    {
      "Code": "CB05",
      "Name": "Mike",
      "Number": "8-888-888-88-88",
      "_links": {
        "self": {
          "href": "/api/clients/CB05"
        },
        "order": {
          "href": "/api/orders/client/CB05"
        }
      }
    }
  ]
}
```
### GraphQL
Request
```gravql
{
  order(code: "AA45") {
    client {
      name
      number
    }
    address
  }
}

```
Response
```json
{
  "data": {
    "order": {
      "client": {
        "name": "Ivan",
        "number": "8-888-888-88-88"
      },
      "address": "1001 Commerce St Dallas TX 75202"
    }
  },
  "extensions": {
    "tracing": {
      "version": 1,
      "startTime": "2023-09-23T08:27:08.6725777Z",
      "endTime": "2023-09-23T08:27:08.6756262Z",
      "duration": 3048600,
      "parsing": {
        "startOffset": 1532800,
        "duration": 30800
      },
      "validation": {
        "startOffset": 1564400,
        "duration": 348400
      },
      "execution": {
        "resolvers": []
      }
    }
  }
}
```
