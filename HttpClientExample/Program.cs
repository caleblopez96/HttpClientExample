using HttpClientExample.Models;
using HttpClientExample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register services with typed HttpClients
        services.AddHttpClient<UserService>();
        services.AddHttpClient<PostService>();
        services.AddHttpClient<CommentService>();
        services.AddHttpClient<AlbumService>();
        services.AddHttpClient<PhotoService>();
        services.AddHttpClient<TodoService>();

        // Register IDbConnection for Dapper
        services.AddTransient<IDbConnection>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        });
    })
    .ConfigureLogging(logging =>
    {
        // clear out the default logging
        logging.ClearProviders();
    })
    .Build();

// Resolve the services from host.Services
var userService = host.Services.GetRequiredService<UserService>();
var postService = host.Services.GetRequiredService<PostService>();
var commentService = host.Services.GetRequiredService<CommentService>();
var albumService = host.Services.GetRequiredService<AlbumService>();
var photoService = host.Services.GetRequiredService<PhotoService>();
var todoService = host.Services.GetRequiredService<TodoService>();

// Run the app logic
//await DisplayUsersData(userService);
//Console.WriteLine();
//await DisplayPostData(postService);
//Console.WriteLine();
//await DisplayPostDataById(postService);
//Console.WriteLine();
//await DisplayComments(commentService);
//Console.WriteLine();
//await DisplayCommentById(commentService);
//Console.WriteLine();
//await DisplayAllAlbums(albumService);
//Console.WriteLine();
//await DisplayAlbumByAlbumId(albumService);
//Console.WriteLine();
//await DisplayAllPhotos(photoService);
//Console.WriteLine();
//await DisplayPhotoById(photoService);
//Console.WriteLine();
//await DisplayAllTodos(todoService);
//Console.WriteLine();
//await DisplayTodosBasedOnStatus(todoService);
//Console.WriteLine();
//await DisplayUserById(userService);
//Console.WriteLine();
//await DisplayUsersFromDb(userService);
//Console.WriteLine();
//await DisplayAllUsersFromApi(userService);
//Console.WriteLine();
//await userService.CompareUsersFromDbAndApi();
//Console.WriteLine();
//await DisplayAndSaveComments(commentService);
//Console.WriteLine();
//await DisplayAllCommentsFromDb(commentService);
//Console.WriteLine();
//await InsertAlbumsIntoDb(albumService);
//Console.WriteLine();
//await DisplayAllAlbumsFromDb(albumService);
await TestSyncComments(commentService);
await TestSyncAlbums(albumService);
//await InsertPhotosIntoDb(photoService);
await TestSyncPhotos(photoService);




// App logic methods

// User data
/*static async Task DisplayUsersData(UserService userService)
{
    var users = await userService.GetAllUsersFromApi();
    Console.WriteLine($"Retrieved {users.Count} users\n");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
        Console.WriteLine();
    }
}*/

/*static async Task DisplayUserById(UserService userService)
{
    int id = 1;
    try
    {
        var user = await userService.GetUserById(id);
        if (user == null || user.Id == 0)
        {
            Console.WriteLine($"No user found with ID {id}");
            return;
        }
        Console.WriteLine($"User found: {user.Id}, {user.Name}, {user.Email}");
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Http Request Exception: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}*/

// post data
/*static async Task DisplayPostData(PostService postService)
{
    var posts = await postService.GetAllPostAsync();
    var topPosts = posts.Take(5);
    Console.WriteLine($"Retrieved {topPosts.Count()} posts\n");
    foreach (var post in topPosts)
    {
        Console.WriteLine($"Post {post.Id}: {post.Title} - {post.Body}");
        Console.WriteLine();
    }
}*/

/*static async Task DisplayPostDataById(PostService postService)
{
    int postId = 1;
    var post = await postService.GetPostByIdAsync(postId);
    if (post != null)
    {
        Console.WriteLine($"Post ID: {post.Id}");
        Console.WriteLine($"User ID: {post.UserId}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
    }
    else
    {
        Console.WriteLine($"No post found by postId {postId}");
    }
}
*/

// comments
/*static async Task DisplayComments(CommentService commentService)
{
    var comments = await commentService.GetAllCommentsFromApi();
    var top10Comments = comments.Take(10);
    Console.WriteLine("First 10 Comments");
    Console.WriteLine();
    foreach (var comment in top10Comments)
    {
        Console.WriteLine($"CommentID: {comment.Id}");
        Console.WriteLine($"Post: {comment.Name}");
        Console.WriteLine($"Post Email: {comment.Email}");
        Console.WriteLine($"Post Body: {comment.Body}");
        Console.WriteLine();
    }
}*/

/*static async Task DisplayCommentById(CommentService commentService)
{
    int postId = 5;
    var comment = await commentService.GetCommentByCommentId(postId);
    if (comment != null)
    {
        Console.WriteLine($"CommentId: {comment.Id}");
        Console.WriteLine($"Comment: {comment.Body}");
    }
    else
    {
        Console.WriteLine($"No comment at comment id {postId}");
    }
}
*/

// albums
/*static async Task DisplayAllAlbums(AlbumService albumService)
{
    var albums = await albumService.GetAllAlbumsFromApi();

    if (albums != null)
    {
        var top10Albums = albums.Take(10);
        Console.WriteLine("First 10 Albums");
        foreach (var album in top10Albums)
        {
            Console.WriteLine($"Album: {album.Title}");
        }
    }
}*/

/*static async Task DisplayAlbumByAlbumId(AlbumService albumService)
{
    int albumId = 5;
    AlbumDto? album = await albumService.GetAlbumById(albumId);
    if (album != null)
    {
        Console.WriteLine($"Album ID: {album.Id}");
        Console.WriteLine($"Album Title: {album.Title}");
    }
    else
    {
        Console.WriteLine($"No album at album ID: {albumId}");
    }
}
*/

// photos
/*static async Task DisplayAllPhotos(PhotoService photoService)
{
    List<PhotoDto> photos = await photoService.GetAllPhotosFromApi();
    var top10Photos = photos.Take(10);
    if (photos != null)
    {
        foreach (var photo in top10Photos)
        {
            Console.WriteLine($"Album ID: {photo.Id} Album Title: {photo.Title}");
        }
    }
    else
    {
        Console.WriteLine("No photos to return");
    }
} */

/* static async Task DisplayPhotoById(PhotoService photoService)
{
    int photoId = 5;
    PhotoDto? photo = await photoService.GetPhotoById(photoId);
    if (photo != null)
    {
        Console.WriteLine($"Photo ID: {photo.Id}, Photo Title: {photo.Title}");
    }
    else
    {
        Console.WriteLine("No photo found.");
    }
}
*/

// todos
/*static async Task DisplayAllTodos(TodoService todoService)
{
    List<TodoDto> todos = await todoService.GetAllTodos();
    var top10Todos = todos.Take(10);
    if (todos != null)
    {
        foreach (var todo in top10Todos)
        {
            Console.WriteLine($"{todo.Id}, {todo.Title}");
        }
    }
    else
    {
        Console.WriteLine("No todos");
    }
}
*/

/*static async Task DisplayTodosBasedOnStatus(TodoService todoService)
{
    var status = TodoService.TodoStatus.Incomplete;
    var todos = await todoService.GetTodosBasedOnStatus(status);
    var top5Todos = todos.Take(5);

    // check for .Any() because the IEnumerable always returns a list(empty or not), so it'll never be null
    if (top5Todos.Any())
    {
        Console.WriteLine($"{status} todos:");
        foreach (var todo in todos)
        {
            Console.WriteLine($"{status}: {todo.Id}, {todo.Title}");
        }
    }
    else
    {
        Console.WriteLine($"{status.ToString().ToLower()} todos found.");
    }
}*/

// db testing
/*static async Task DisplayUsersFromDb(UserService userService)
{
    var users = await userService.GetAllUsersFromDb();
    Console.WriteLine($"[Dapper] Retrieved {users.Count} users from the database\n");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
        Console.WriteLine();
    }
}
*/

/*static async Task DisplayAllUsersFromApi(UserService userService)
{
    var users = await userService.GetAllUsersFromApi();
    Console.WriteLine($"Retrieved {users.Count} users from API");
}*/

/*static async Task DisplayAllCommentsFromApi(CommentService commentService)
{
    var comments = await commentService.GetAllCommentsFromApi();
    Console.WriteLine($"Retrieved {comments.Count} comments");
}
*/

/*static async Task DisplayAllCommentsFromDb(CommentService commentService)
{
    var comments = await commentService.GetAllCommentsFromDb();
    Console.WriteLine($"Retrieved {comments.Count} from the db");
    foreach (var comment in comments)
    {
        Console.WriteLine($"Comment ID: {comment.Id}, Comment Body: {comment.Body}");
    }
}*/

/*static async Task DisplayAllAlbumsFromDb(AlbumService albumService)
{
    var albums = await albumService.GetAllAlbumsFromDb();
    foreach (var album in albums)
    {
        Console.WriteLine($"Album: {album.Title}");
        Console.WriteLine($"Id: {album.Id}");
        Console.WriteLine($"UserId: {album.UserId}");
    }
}*/

// syncing test
static async Task TestSyncComments(CommentService commentService)
{
    await commentService.SyncCommentsWithApi();
}

static async Task TestSyncAlbums(AlbumService albumService)
{
    await albumService.SyncAlbumsWithApi();
}

static async Task TestSyncPhotos(PhotoService photoService)
{
    await photoService.SyncPhotosWithApi();
}

// populating db tables

/*static async Task InsertAlbumsIntoDb(AlbumService albumService)
{
    var albums = await albumService.GetAllAlbumsFromApi();

    var insertedAlbums = await albumService.InsertAlbumbsIntoDb(albums);

    Console.WriteLine($"Inserted {insertedAlbums.Count} albums into the database.");
}
*/

/*static async Task InsertPhotosIntoDb(PhotoService photoService)
{
    var photos = await photoService.GetAllPhotosFromApi();
    await photoService.InsertPhotosIntoDb(photos);
    Console.WriteLine($"Inserted {photos.Count} into the db");
}
*/