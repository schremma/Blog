namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorToPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Post", "Author_Id", c => c.String(maxLength: 4000));
            CreateIndex("Blog.Post", "Author_Id");
            AddForeignKey("Blog.Post", "Author_Id", "dbo.Blog_Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Blog.Post", "Author_Id", "dbo.Blog_Users");
            DropIndex("Blog.Post", new[] { "Author_Id" });
            DropColumn("Blog.Post", "Author_Id");
        }
    }
}
