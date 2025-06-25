using HttpClientExample.Models;
using HttpClientExample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        // Register services with typed HttpClients
        services.AddHttpClient<UserService>();
        services.AddHttpClient<PostService>();
        services.AddHttpClient<CommentService>();
        services.AddHttpClient<AlbumService>();
        services.AddHttpClient<PhotoService>();
    }).ConfigureLogging(logging =>
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

// Run the app logic
await DisplayUsersData(userService);
Console.WriteLine();
await DisplayPostData(postService);
Console.WriteLine();
await DisplayPostDataById(postService);
Console.WriteLine();
await DisplayComments(commentService);
Console.WriteLine();
await DisplayCommentById(commentService);
Console.WriteLine();
await DisplayAllAlbums(albumService);
Console.WriteLine();
await DisplayAlbumByAlbumId(albumService);
Console.WriteLine();
await DisplayAllPhotos(photoService);
Console.WriteLine();
await DisplayPhotoById(photoService);

// App logic methods
static async Task DisplayUsersData(UserService userService)
{
    var users = await userService.GetAllUsers();
    Console.WriteLine($"Retrieved {users.Count} users\n");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
        Console.WriteLine();
    }
}

static async Task DisplayPostData(PostService postService)
{
    var posts = await postService.GetAllPostAsync();
    var topPosts = posts.Take(5);
    Console.WriteLine($"Retrieved {topPosts.Count()} posts\n");
    foreach (var post in topPosts)
    {
        Console.WriteLine($"Post {post.Id}: {post.Title} - {post.Body}");
        Console.WriteLine();
    }
}

static async Task DisplayPostDataById(PostService postService)
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

static async Task DisplayComments(CommentService commentService)
{
    var comments = await commentService.GetAllComments();
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
}

static async Task DisplayCommentById(CommentService commentService)
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

static async Task DisplayAllAlbums(AlbumService albumService)
{
    var albums = await albumService.GetAllAlbumsAsync();

    if (albums != null)
    {
        var top10Albums = albums.Take(10);
        Console.WriteLine("First 10 Albums");
        foreach (var album in top10Albums)
        {
            Console.WriteLine($"Album: {album.Title}");
        }
    }
}

static async Task DisplayAlbumByAlbumId(AlbumService albumService)
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

static async Task DisplayAllPhotos(PhotoService photoService)
{
    List<PhotoDto> photos = await photoService.GetAllPhotosAsync();
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
}

static async Task DisplayPhotoById(PhotoService photoService)
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