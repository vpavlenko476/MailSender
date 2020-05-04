using System.ComponentModel.DataAnnotations;


namespace MailSender.DAL.Models.Base
{
	public class EntityBase
	{
		[Key]
		public int Id { get; set; }
	}
}
