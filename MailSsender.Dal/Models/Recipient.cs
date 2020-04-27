using MailSender.DAL.Models.Base;

namespace MailSender.DAL.Models
{
	/// <summary>
	/// Получатель письма
	/// </summary>
	public class Recipient: EntityBase
	{
		/// <summary>
		/// Имя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }
	}
}
