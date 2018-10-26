namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Blog.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 4000),
                        ParentCommentId = c.Int(),
                        Content = c.String(nullable: false, maxLength: 4000),
                        CreateDate = c.DateTime(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Email = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog_Users", t => t.ApplicationUserId)
                .ForeignKey("Blog.Comment", t => t.ParentCommentId)
                .ForeignKey("Blog.Post", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ParentCommentId);
            
            CreateTable(
                "dbo.Blog_Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 4000),
                        UserName = c.String(maxLength: 4000),
                        PasswordHash = c.String(maxLength: 4000),
                        SecurityStamp = c.String(maxLength: 4000),
                        Email = c.String(maxLength: 4000),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blog_UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(maxLength: 4000),
                        ClaimValue = c.String(maxLength: 4000),
                        User_Id = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog_Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Blog_UserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 4000),
                        LoginProvider = c.String(nullable: false, maxLength: 4000),
                        ProviderKey = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Blog_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Blog_UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 4000),
                        RoleId = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Blog_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Blog_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Blog_Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Blog.Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titel = c.String(nullable: false, maxLength: 150),
                        Content = c.String(nullable: false, maxLength: 4000),
                        CreateDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                        PublishDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Blog.Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileBytes = c.Binary(maxLength: 4000),
                        ContentType = c.String(maxLength: 4000),
                        FileName = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourcePosts",
                c => new
                    {
                        Resource_Id = c.Int(nullable: false),
                        Post_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Resource_Id, t.Post_Id })
                .ForeignKey("Blog.Resource", t => t.Resource_Id, cascadeDelete: true)
                .ForeignKey("Blog.Post", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Resource_Id)
                .Index(t => t.Post_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourcePosts", "Post_Id", "Blog.Post");
            DropForeignKey("dbo.ResourcePosts", "Resource_Id", "Blog.Resource");
            DropForeignKey("Blog.Comment", "PostId", "Blog.Post");
            DropForeignKey("Blog.Comment", "ParentCommentId", "Blog.Comment");
            DropForeignKey("Blog.Comment", "ApplicationUserId", "dbo.Blog_Users");
            DropForeignKey("dbo.Blog_UserClaims", "User_Id", "dbo.Blog_Users");
            DropForeignKey("dbo.Blog_UserRoles", "UserId", "dbo.Blog_Users");
            DropForeignKey("dbo.Blog_UserRoles", "RoleId", "dbo.Blog_Roles");
            DropForeignKey("dbo.Blog_UserLogins", "UserId", "dbo.Blog_Users");
            DropIndex("dbo.ResourcePosts", new[] { "Post_Id" });
            DropIndex("dbo.ResourcePosts", new[] { "Resource_Id" });
            DropIndex("dbo.Blog_UserRoles", new[] { "RoleId" });
            DropIndex("dbo.Blog_UserRoles", new[] { "UserId" });
            DropIndex("dbo.Blog_UserLogins", new[] { "UserId" });
            DropIndex("dbo.Blog_UserClaims", new[] { "User_Id" });
            DropIndex("Blog.Comment", new[] { "ParentCommentId" });
            DropIndex("Blog.Comment", new[] { "ApplicationUserId" });
            DropIndex("Blog.Comment", new[] { "PostId" });
            DropTable("dbo.ResourcePosts");
            DropTable("Blog.Resource");
            DropTable("Blog.Post");
            DropTable("dbo.Blog_Roles");
            DropTable("dbo.Blog_UserRoles");
            DropTable("dbo.Blog_UserLogins");
            DropTable("dbo.Blog_UserClaims");
            DropTable("dbo.Blog_Users");
            DropTable("Blog.Comment");
        }
    }
}
