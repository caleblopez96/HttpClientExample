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


[X] Comment service
	[X] get data from api
	[X] get data from db
	[X] insert data (to be used in sync and to popluate table)
	[X] update data ()
	[X] helper method to check if objects are equal
	[X] sync method

[] User service
	[] get data from api
	[] get data from db
	[] insert data (to be used in sync and to popluate table)
	[] update data ()
	[] helper method to check if objects are equal
	[] sync method

[] Album Service (still need to insert data into db)
	[X] get data from api
	[X] get data from db
	[X] insert data (to be used in sync and to popluate table)
	[] update data ()
	[X] helper method to check if objects are equal
	[X] sync method

[] Photo Service (still need to insert data into db )
	[] get data from api
	[] get data from db
	[] insert data (to be used in sync and to popluate table)
	[] update data ()
	[] helper method to check if objects are equal
	[] sync method

[] Post Service (still need to insert data into db)
	[] get data from api
	[] get data from db
	[] insert data (to be used in sync and to popluate table)
	[] update data ()
	[] helper method to check if objects are equal
	[] sync method

[] Todo Service (still need to insert data into db)
	[] get data from api
	[] get data from db
	[] insert data (to be used in sync and to popluate table)
	[] update data ()
	[] helper method to check if objects are equal
	[] sync method

[] User Service (still need to insert data into db)
	[] get data from api
	[] get data from db
	[] insert data (to be used in sync and to popluate table)
	[] update data ()
	[] helper method to check if objects are equal
	[] sync method
