using System.Data.Entity;

namespace MailSender.DAL.Models.Context
{
	public class MailSenderContext: DbContext
	{
		public MailSenderContext() : base() { }

		///<inheritdoc cref="MailSender.DAL.Models.Sender"/>
		public DbSet<Sender> Senders { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.Recipient"/>
		public DbSet<Recipient> Recipients { get; set; }

		///<inheritdoc cref="MailSender.DAL.Models.Host"/>
		public DbSet<Host> Hosts { get; set; }
	}
}
