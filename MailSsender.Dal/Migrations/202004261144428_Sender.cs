namespace MailSender.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Senders", "Password", c => c.Guid(nullable: false));
            DropColumn("dbo.Senders", "EncryptPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Senders", "EncryptPassword", c => c.String());
            DropColumn("dbo.Senders", "Password");
        }
    }
}
