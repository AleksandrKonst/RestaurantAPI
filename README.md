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
