using HttpClientExample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        // Register services with typed HttpClients
        services.AddHttpClient<UserService>();
        services.AddHttpClient<PostService>();
        services.AddHttpClient<CommentService>();
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

// Run the app logic
await GetUsersData(userService);
Console.WriteLine();
await GetPostData(postService);
Console.WriteLine();
await GetPostDataById(postService);
Console.WriteLine();
await GetComments(commentService);
Console.WriteLine();
await GetCommentById(commentService);

// App logic methods
static async Task GetUsersData(UserService userService)
{
    var users = await userService.GetAllUsers();
    Console.WriteLine($"Retrieved {users.Count} users\n");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
        Console.WriteLine();
    }
}

static async Task GetPostData(PostService postService)
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

static async Task GetPostDataById(PostService postService)
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

static async Task GetComments(CommentService commentService)
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

static async Task GetCommentById(CommentService commentService)
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
