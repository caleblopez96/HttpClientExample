# TODO

**HIGH LEVEL OVERVIEW**

# Service Build Checklist

**For each service (Comments, Users, Albums, Photos, Posts, Todos):**

## Core Methods  
`GetDataFromApi()`
- [ ] **Get Data From API**
  - [ ] Define API endpoint  
  - [ ] Fetch JSON data using `GetFromJsonAsync()`  
  - [ ] Handle exceptions  

`GetDataFromDb()`
- [ ] **Get Data From DB**
  - [ ] Establish DB connection  
  - [ ] Write `SELECT` query  
  - [ ] Await query result  
  - [ ] Return as list  

`InsertDataIntoDb()`
- [ ] **Insert Data Into DB**
  - [ ] Establish DB connection  
  - [ ] Write `INSERT` query  
  - [ ] Loop through collection  
  - [ ] Execute query per object  
  - [ ] Return inserted objects  

`UpdateDataInDb()`
- [ ] **Update Data In DB**
  - [ ] Establish DB connection  
  - [ ] Write `UPDATE` query  
  - [ ] Loop through collection  
  - [ ] Execute query per object  

`DeleteFromDb()`
- [ ] **Delete Data From DB** *(if needed for full sync)*
  - [ ] Establish DB connection  
  - [ ] Write `DELETE` query  
  - [ ] Execute query  

`ObjectsAreEqual()`
- [ ] **ObjectsAreEqual Helper**
  - [ ] Compare each relevant property  
  - [ ] Return `true` if equal  

`SyncDataWithApi()`
- [ ] **Sync Method**
  - [ ] Fetch data from API  
  - [ ] Fetch data from DB  
  - [ ] Build `newObjects` list for missing records  
  - [ ] Build `updatedObjects` list for changed records  
  - [ ] Optionally, build `deletedObjects` list for removed records  
  - [ ] Insert new records  
  - [ ] Update changed records  
  - [ ] Delete removed records (if applicable)  
  - [ ] Log results  

## Service Implementation Progress  

| Service       | Get From API | Get From DB | Insert | Update | Delete | AreEqual | Sync |
|:--------------|:--------------|:------------|:---------|:--------|:---------|:------------|:-------|
| ✅ CommentService | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| 🔲 UserService   | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 |
| ✅ AlbumService  | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| 🔲 PhotoService  | ✅ | ✅ | ✅ | 🔲 | 🔲 | 🔲 | 🔲 |
| 🔲 PostService   | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 |
| 🔲 TodoService   | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 | 🔲 |





---

API and Endpoints:
[X] GET	/posts	List all posts
[X] GET	/posts/{id}	Get a single post by ID
[X] GET	/comments	List all comments
[X] GET	/comments/{id}	Get a single comment by ID
[X] GET	/albums	List all albums
[X] GET	/albums/{id}	Get a single album by ID
[X] GET	/photos	List all photos
[X] GET	/photos/{id}	Get a single photo by ID
[X] GET	/todos	List all todos
[X] GET	/todos/{id}	Get a single todo by ID
[X] GET	/users	List all users
[X] GET	/users/{id}	Get a single user by ID

Incorporate Logging:
[] Incorporate Serilogger

Connect to DB:
[X] connect to database using dapper