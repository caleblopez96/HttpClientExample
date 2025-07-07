# HIGH LEVEL OVERVIEW

## Do this for each service:
1. Create your service class
	- Define constructor using constructor injection to receive dependencies
	- `HttpClient` allows you to make/handle http request
	- `IConfiguration` allows you to access your app settings and connection strings
```csharp
public class NameOfService(HttpClient client, IConfiguration configuration)
{
	// rest of class goes inside of here
}
```

2. Init fields using constructor parameters
	- Assign `HttpClient` to a private readonly field (i.e., _client) 
	- Set a private readonly `baseUrl` for api calls
	- Use `configuration.GetConnectionString("DefaultConnection")` to assign the db connection
	- *OPTIONAL: configure endpoint as a variable at top level*
		- *OR configure the endpoint in each method* 
```csharp
public class NameOfService(HttpClient client, IConfiguration configuration)
{
	private readonly HttpClient _client = client;
	private readonly baseUrl = "https://jsonplaceholder.typicode.com";
	private readonly _connectionString = configration.GetConnectionString("DefaultConnection");
}
```

3.	Create necessary methods following necessary workflow
	1. `InsertObjectsIntoDb()` - Inserts information from api to db (so db can be populated with info)

	2. `GetAllObjectsFromApi` - Gets all the objects from the api and will be used on one side of the comparison.	

	3. `GetAllObjectsFromDb` - Gets all the objects from the db and will be used as the other side of the comparison.

	4. `InsertObjectsIntoDb` - Inserts objects into the db if they dont already exist

	5. `UpdateObjectsInDb` - Updates Objects in the db that need updating
	
	6. `ObjectsAreEqual` - Helper method to check if objects are equal

- `InsertObjectsIntoDb()`
	- Establish a connection to the db
	- Write insert query
	- Foreach object inside the collection, execute the query on it
	- Return the result
```csharp
public async Task<List<ObjectDto>> InsertObjectsIntoDb(List<ObjectDto> objects)
{
	// establish connection with the database
	using var connection = new SqlConnection(_connectionString);

	// write insert query
	string query = @"INSERT INTO TableName (column1, column2, column3)
					 VALUES (@value1, @value2, @value3)";

	// loop over objects inside collection
	foreach (var object in objects)
	{
		await connection.ExecuteAsync(query, object);
	}
	// return the results
	return objects;
}
```

- `GetObjectsFromApi()`
	- Define your endpoint
	- Try the call to the api
		- if it doesnt return objects, return empty object (`[]`) 
	- Catch and handle the exceptions

```csharp
public async Task<List<ObjectDto>> GetObjectsFromApi()
{
	// define endpoint
	string endpoint = "/users"

	try 
	{
	    // call the api
		List<ObjectDto> objects = await _client.GetFromJsonAsync<List<ObjectDto>>(baseUrl + endpoint);
		return objects ?? [];
	}

	// handle HttpRequestException
	catch (HttpRequestException ex)
	{
		Console.WriteLine($"HTTP Error: {ex.Message}");
		return [];
	}

	// handle JsonException 
	catch (JsonException ex)
	{
		Console.WriteLine($"Json Error: {ex.Message}");
		return [];
	}

	// handle exceptions
	catch (Exception ex)
	{
		Console.WriteLine(#"Error: {ex.Message}");
		return [];
	}
}
```

- `GetObjectsFromDb()`
	- Establish connection with the db using the connection string
	- Write SELECT query
	- Await the response
	- Return the result

```csharp
public async Task<List<ObjectDto>> GetAllObjectsFromDb()
{
	// establish connection with the db
	using var connection = new SqlConnection(_connectionString);

	// write select query
	string query = @"SELECT Column1, Column2, ColumnN FROM TableName";
	
	// await response
	IEnumerable<ObjectDto> objectsInDb = await connection.QueryAsync<ObjectDto>(query);

	// return the result
	return objectsInDb.ToList();
}
```


- `InsertObjectsIntoDb()`
	- Establish connection with the db using the connection string
	- Write INSERT statement
	- Loop through the collection of objects
		- foreach object in the collection, await the connection and execute the query
```csharp
public async Task InsertObjects(List<ObjectDto> objects)
{
	// establish connection with the db
	using var connection = new SqlConnection(_connectionString);

	// write insert statement
	string query = @"INSERT INTO TableName (Column1, Column2, ColumnN)
					 VALUES (@Column1, @Column2, @ColumnN)";

	// 
	foreach (var object in objects)
	{
		await connection.ExecuteAsync(query, objects)
	}
}
```

- `UpdateObjectsIntoDb()`
	- Establish connection with the db using the connection string
	- Write UPDATE statement
	- Loop through the collection of objects
		- foreach object in the collection, await the connection and execute the querys
```csharp
public async Task UpdateObjects(List<ObjectDto> objects)
{
	using var connection = new SqlConnection(_connectionString);
	string query = @"UPDATE TableName
					 SET Column1 = @Column1
					 Column2 = @Column2
					 ColumnN = @ColumnN";
	foreach (object in objects)
	{
		await connection.ExecuteAsync(query, objects);
	}
}
```

- `ObjectsAreEqual()`
	- Create method that takes in two objects for comparison
	- Code comparison
```csharp
private bool ObjectsAreEqual(ObjectDto object1, ObjectDto object2)
{
	return object1.property1 == object2.property1 &&
		   object1.property2 == object2.property2 &&
		   object1.propertyN == object2.propertyN;
}
```

- `SyncObjectsFromApiToDb()`
  - Get objects from the database (e.g. call `GetAllObjectsFromDb()`)
  - Get objects from the API (e.g. call `GetAllObjectsFromApi()`)
  
  - Create a `newObjects` list to hold new records from the API not in the database
  - Create an `updatedObjects` list to hold records that exist in the DB but have different values in the API
  
  - `foreach` object in the API results:
    - Check if the object exists in the database (use `FirstOrDefault` by Id or unique key)
    - If it doesn't exist in the DB:
      - Add it to `newObjects`
    - Else if it exists but has differences (compare using a helper method like `ObjectsAreEqual()`):
      - Add it to `updatedObjects`

  - If `newObjects` has any records:
    - Call `InsertObjects(newObjects)`
    - Log how many were inserted

  - If `updatedObjects` has any records:
    - Call `UpdateObjects(updatedObjects)`
    - Log how many were updated

  - If both `newObjects` and `updatedObjects` are empty:
    - Log "No changes detected"
```csharp
public async Task SyncObjectsFromApiToDb()
{
    // get objects from API and DB
    var apiObjects = await GetAllObjectsFromApi();
    var dbObjects = await GetAllObjectsFromDb();

    // new and updated object lists
    var newObjects = new List<TObject>();
    var updatedObjects = new List<TObject>();

    // compare each API object to DB object
    foreach (var apiObject in apiObjects)
    {
        var dbObject = dbObjects.FirstOrDefault(o => o.Id == apiObject.Id);
        if (dbObject == null)
        {
            newObjects.Add(apiObject);
        }
        else if (!ObjectsAreEqual(apiObject, dbObject))
        {
            updatedObjects.Add(apiObject);
        }
    }

    // insert new objects
    if (newObjects.Count > 0)
    {
        await InsertObjects(newObjects);
        Console.WriteLine($"Inserted {newObjects.Count} records");
    }

    // update changed objects
    if (updatedObjects.Count > 0)
    {
        await UpdateObjects(updatedObjects);
        Console.WriteLine($"Updated {updatedObjects.Count} records");
    }

    // log no changes
    if (newObjects.Count == 0 && updatedObjects.Count == 0)
    {
        Console.WriteLine("No changes detected");
    }
}

```
