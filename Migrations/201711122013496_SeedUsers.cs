namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3cfc0973-0fa2-49ca-8238-471b36444f9f', N'guest@vidly.com', 0, N'ABBhBbf9POGZSBWHZxCmYgNbupywPuwM0a4LaV/DmV5yYu/3bvwhKlf3Q9GOkbxg+Q==', N'fb1a1fb3-8a51-4dc2-af09-21b6ab0185e0', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'f726262a-6c1f-410c-a370-ad9446146df6', N'admin@vidly.com', 0, N'ABhtIvDwCjQNXU2XioFGca7iR3luiipmltU8Op8ra+FI9xc8CQbG/Wa3+lcjxneK9w==', N'75078f61-a614-449b-83f5-31198becef4a', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')

            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'48c49a6b-cb69-495a-8182-488017af09c0', N'CanManageMovies')

            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f726262a-6c1f-410c-a370-ad9446146df6', N'48c49a6b-cb69-495a-8182-488017af09c0')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
