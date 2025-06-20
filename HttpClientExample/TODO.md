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
GET	/comments/{id}	Get a single comment by ID
GET	/albums	List all albums
GET	/albums/{id}	Get a single album by ID
GET	/photos	List all photos
GET	/photos/{id}	Get a single photo by ID
GET	/todos	List all todos
GET	/todos/{id}	Get a single todo by ID
GET	/users	List all users
GET	/users/{id}	Get a single user by ID