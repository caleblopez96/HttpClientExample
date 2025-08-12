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
| ✅ CommentService| ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| ✅ UserService   | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| ✅ AlbumService  | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| ✅ PhotoService  | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| ✅ PostService   | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| ✅ TodoService   | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |





---

JSON Placeholder API and Endpoints:
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





-----
proposed next steps: 

# Full-Stack Sync Project - TODO List
TODO:
✅ Finish last sync service (comments, etc.).

[] Create api to access db. api needs to include endpoints listed in ##backend tasks.

[] Build out React frontend one feature/page at a time.

## BACKEND TASKS

✅ Create REST API endpoints for:
[] GET /api/albums — return all albums from DB
[] GET /api/comments — return all comments from DB
[] GET /api/photos — return all photos from DB
[] GET /api/posts — return all posts from DB
[] GET /api/todo — return all todos from DB
[] GET /api/users — return all users from DB
[] POST /api/sync/posts — sync posts from JSONPlaceholder to your DB
[] POST /api/sync/users — sync users
[] POST /api/sync/comments — sync comments
[] POST /api/sync/albums — sync albums
[] POST /api/sync/photos — sync photos
[] POST /api/sync/todo — sync todos
[] GET /api/posts/{id} — return a single post by ID
[] GET /api/users/{id} — return a single user by ID
[] POST /api/posts — create a new post
[] PUT /api/posts/{id} — update an existing post
[] DELETE /api/posts/{id} — delete a post by ID
[] POST /api/users — create a new user
[] PUT /api/users/{id} — update a user
[] DELETE /api/users/{id} — delete a user by ID

---

## FRONTEND (React)

- [ ] ✅ Initialize React app using Vite or Bun (I like vite)
  - [ ] Install Axios

- [ ] ✅ Create components for each resource:
  - [ ] `<Posts />`
  - [ ] `<Users />`
  - [ ] `<Comments />`

- [ ] ✅ In each component:
  - [ ] Fetch data from backend API
  - [ ] Display list of data
  - [ ] Show loading/error states

- [ ] ✅ Create a "Sync Now" button:
  - [ ] Call `POST /api/sync/[resource]` when clicked
  - [ ] Refresh data after syncing

---

## 🧠 BONUS ENHANCEMENTS

- [ ] Add table sorting / filtering on frontend
- [ ] Display sync logs or last synced time
- [ ] Add a search bar
- [ ] Add pagination if data gets large
- [ ] Create a combined "Sync All" button (triggers all syncs)
- [ ] Add loading spinner when syncing
- [ ] Add styling (e.g., TailwindCSS or Bootstrap)

---

## 🛡️ FUTURE IDEAS (Optional)

- [ ] Add authentication (JWT login or simple token-based)
- [ ] Add unit tests to backend sync logic
- [ ] Dockerize backend and database
- [ ] Deploy backend (e.g., Render, Railway, Fly.io)
- [ ] Deploy frontend (e.g., Vercel, Netlify)



8. Add Unit Tests
Test your sync logic:

Write tests for CRUD comparison logic.

Mock the API.

Verify your app behaves as expected.