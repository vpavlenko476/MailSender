using Entities;
using System.Data.Entity;

namespace Data.Context
{
	public class MailSenderContext: DbContext
	{
		public MailSenderContext() : base() { }

		///<inheritdoc cref="MailSender.DAL.Models.Sender"/>
		public DbSet<SenderEntity> Senders { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.Recipient"/>
		public DbSet<RecipientEntity> Recipients { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.Host"/>
		public DbSet<HostEntity> Hosts { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.Message"/>
		public DbSet<MessageEntity> Messages { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.RecipientMessage"/>
		public DbSet<Recipient2MessageEntity> RecipientMessages { get; set; }
	}
}
