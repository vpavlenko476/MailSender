namespace MailSender.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageMode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        Title = c.String(),
                        Body = c.String(),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Senders", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.RecipientMessages",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        Message_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.Message_Id })
                .ForeignKey("dbo.Recipients", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.Message_Id, cascadeDelete: true)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Message_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Senders");
            DropForeignKey("dbo.RecipientMessages", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.RecipientMessages", "Recipient_Id", "dbo.Recipients");
            DropIndex("dbo.RecipientMessages", new[] { "Message_Id" });
            DropIndex("dbo.RecipientMessages", new[] { "Recipient_Id" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropTable("dbo.RecipientMessages");
            DropTable("dbo.Messages");
        }
    }
}
