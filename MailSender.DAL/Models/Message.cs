using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.DAL.Models
{
	public class Message
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
	}
}
