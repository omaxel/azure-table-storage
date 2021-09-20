# Todos
This is an example .NET 5 MVC application that stores data on [Azure Table Storage](https://docs.microsoft.com/en-us/azure/storage/tables/table-storage-overview). It allows managing Todos.

The list is paginated using a [Continuation Token](https://docs.microsoft.com/en-us/rest/api/storageservices/query-timeout-and-pagination).

## Getting started
You have to update the connection string which points to the Azure Storage Account in the `appsettings.json` file. The table name must be *Todo* since it's chosen by the entity name.

You can then run the application using:

```
dotnet run --launch-profile Todos
```

## Known issues and limitations
- The list of elements is not ordered since is [not supported](https://docs.microsoft.com/en-us/rest/api/storageservices/Query-Operators-Supported-for-the-Table-Service) by Azure Table Storage.
- APIs to add and remove Todos don't return the modified entity and the same it's not updated. It should be fetched again by the source.