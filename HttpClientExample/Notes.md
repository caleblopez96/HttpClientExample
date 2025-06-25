# Overall Architecture
The app follows a layered architecture with clear separation of concerns:

## DTOs (Data Transfer Objects):
These are simple classes that model the JSON data structure returned by the external API.

They are used exclusively for data deserialization.

No business logic here, just properties matching the API JSON.

## Services Layer:
This layer contains service classes like UserService and PostService.

Each service is responsible for communicating with a specific API endpoint.

Services use HttpClient to make HTTP requests.

They deserialize JSON responses into DTOs using HttpClient extension methods like GetFromJsonAsync<T>().

The services expose asynchronous methods (getAllUsers(), getAllPost()) that return strongly typed collections.

## Program / Application Layer:
This is the console app entry point (Program.cs) that:

Configures dependency injection (DI) and logging via the Host builder.

Registers services and typed HttpClient instances with DI.

Resolves services from the DI container to call their API methods.

Implements application logic to call services, receive DTO data, and display or process it.

## Dependency Injection & HttpClientFactory
The app uses .NET Generic Host to provide DI and logging support in a console app.

Host.CreateDefaultBuilder() configures the default environment, logging, and DI container.

Services are registered with services.AddHttpClient<T>() which:

Registers a typed HttpClient for each service.

Manages HttpClient lifetimes and handlers efficiently (HttpClientFactory).

Services receive HttpClient instances injected automatically by DI.

At runtime, Program resolves service instances from the DI container to invoke API calls.

### Flow of a Typical API Call
Program calls a service method, e.g., userService.getAllUsers().

Service uses its injected HttpClient to send an HTTP GET request to the configured API endpoint.

HttpClient fetches JSON data from the external API.

The JSON response is deserialized into a list of DTO objects using GetFromJsonAsync<List<UserDto>>().

The service method returns the typed DTO collection back to the caller.

Program processes the data, e.g., loops through users and outputs to console.

## Benefits of this Architecture
Separation of concerns:
API communication is isolated in services, presentation/output logic stays in Program.

Testability:
Services can be easily mocked or tested separately by injecting fake HttpClients or mock data.

Scalability:
Adding new API endpoints is simple — create a new DTO and service, register it with DI.

HttpClient management:
Using HttpClientFactory via AddHttpClient() avoids socket exhaustion issues and improves performance.

Logging & Configuration:
The generic host provides built-in structured logging and easy configuration extension.

### Notes:
What Postman Does in Your Workflow:
Test API endpoints manually
Before you wire them into your app, you can send test requests (GET, POST, PUT, DELETE etc) and see what the API responds with — headers, status codes, and most importantly, the JSON response body.