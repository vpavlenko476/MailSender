
using System.ComponentModel.DataAnnotations.Schema;

namespace MailSender.DAL.Models
{
	[NotMapped]
	public class Message
	{
		public string SenderEmail { get; set; }
		public string RecipientEmail { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
	}
}
