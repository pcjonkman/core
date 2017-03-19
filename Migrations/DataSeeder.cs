using Core.Context;

namespace Core.Migrations
{
  public class DataSeeder
  {

    public static void Initialize(CoreContext context)
    {
        var testUser1 = new Models.User
        {
            Id = "u1",
            FirstName = "Jan",
            LastName = "Janssen"
        };

        context.Users.Add(testUser1);

        var testUser2 = new Models.User
        {
            Id = "u2",
            FirstName = "Karel",
            LastName = "Karels"
        };

        context.Users.Add(testUser2);

        var testPost1 = new Models.Post
        {
            Id = "p1",
            UserId = testUser1.Id,
            Content = "What a piece of junk!"
        };

        context.Posts.Add(testPost1);

        var testPost2 = new Models.Post
        {
            Id = "p2",
            UserId = testUser2.Id,
            Content = "More junk!"
        };

        context.Posts.Add(testPost2);

        var testPost3 = new Models.Post
        {
            Id = "p3",
            UserId = testUser2.Id,
            Content = "A lot of junk!"
        };

        context.Posts.Add(testPost3);

        context.SaveChanges();
    }
  }
}
