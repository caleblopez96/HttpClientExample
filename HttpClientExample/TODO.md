Possible Next Steps / Enhancements
Add configuration support (e.g., base URLs in appsettings.json).

Add error handling and retry policies in HttpClient registrations.

Introduce a repository or domain layer if business logic grows.

Add unit tests mocking the services and HttpClient.

Use IHostedService to run background tasks if needed.

Method	Endpoint	Description
[X] GET	/posts	List all posts
[X] GET	/posts/{id}	Get a single post by ID
[X] GET	/comments	List all comments
[X] GET	/comments/{id}	Get a single comment by ID
[X] GET	/albums	List all albums
[X] GET	/albums/{id}	Get a single album by ID
[X] GET	/photos	List all photos
[X] GET	/photos/{id}	Get a single photo by ID
GET	/todos	List all todos
GET	/todos/{id}	Get a single todo by ID
GET	/users	List all users
GET	/users/{id}	Get a single user by ID

[] Incorporate Serilogger

[X] connect to database using dapper
[] sync all users from jsonplaceholder to users table
[] figure out how to flatten dto into one object and then how to insert it into db. 
rn the way i have my db set up and the way the dto is arent the same. need to create a mapper or something.
