# BankAccountAPI

An ASP .Net Core API which is periodically downloading bank account statements from your different bank accounts and storing them in a Mongo DB.
You can retreive your statements within a date range specified in your request.

## Build and start API (in Docker container)

1. Open ``BankAccountAPI/appsettings.json``and set ``ConnectionString`` to the database connection string of your MongoDB.
2. Run ``cd BankAccountAPI``
3. Run ``docker build -t bankaccountapi .``
4. Run ``docker run -p 80:80 -t bankaccountapi``

### Health Check

Check if the API is running and is healthy by sending a get request to ``http://localhost:80/AccountStatement/HealthCheck``.

## Use API

### Start Collecting Bank Account Statements

The API has to be started once in order to start the periodic downloading process and get your bank credentials (the credentials will not be stored persistently).

Send a post request to ``http://localhost:80/AccountStatement/StartCollecting`` with the following payload:

```json
[
    {
        "bankNumber": "123", // BLZ (in German)
        "accountNumber": "xyz", // Kontonummer (in German)
        "userId": "my_user_login", // Login for online banking
        "pin": "my_secret_pin", // pin for online banking
        "httpsEndpoint": "my_https_endpoint_to_bank", // Fin endpoint of bank
        "bankId": "someBankId" // arbitrary bankId chosen by you
    },
    // more objects...
]
```

Here you have one entry for each of your bank accounts. The ``bankId``is an arbitrary identifier chosen by you which differentiates your different bank accounts. The ``bankId``will be stored within each bank account statement in the database.

### Retreiving collected Bank Account Statements

Send a get request to:

```url
http://localhost:80/AccountStatement?startDate=2021-1-4&endDate=2021-4-3&bankIds=bankId1,bankId2
```

where you specify your start and end date, respectively. The date format is ``yyyy-M-d``.
Moreover, you specify the bank ids for which you want to retreive statements. Those must correspond to the ``bankId`` values from above.
