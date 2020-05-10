using Entities.Abstract;

namespace Entities
{
	/// <summary>
	/// Подключение к smtp-серверу
	/// </summary>
	public class HostEntity: EntityBase
	{
		/// <summary>
		/// Сервер
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Порт
		/// </summary>
		public int Port { get; set; }
	}
}
