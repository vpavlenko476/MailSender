namespace MailSender.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SenderEdit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Senders", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Senders", "Password", c => c.Guid(nullable: false));
        }
    }
}
