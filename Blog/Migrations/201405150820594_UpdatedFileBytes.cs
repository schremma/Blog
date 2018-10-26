namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFileBytes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Blog.Resource", "FileBytes", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("Blog.Resource", "FileBytes", c => c.Binary(maxLength: 4000));
        }
    }
}
