namespace MailSender.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRecipientMessagesToContext : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RecipientMessages", newName: "RecipientMessage1");
            CreateTable(
                "dbo.RecipientMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecipientId = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .ForeignKey("dbo.Recipients", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.RecipientId)
                .Index(t => t.MessageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipientMessages", "RecipientId", "dbo.Recipients");
            DropForeignKey("dbo.RecipientMessages", "MessageId", "dbo.Messages");
            DropIndex("dbo.RecipientMessages", new[] { "MessageId" });
            DropIndex("dbo.RecipientMessages", new[] { "RecipientId" });
            DropTable("dbo.RecipientMessages");
            RenameTable(name: "dbo.RecipientMessage1", newName: "RecipientMessages");
        }
    }
}
