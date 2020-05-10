using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Abstract
{
	public abstract class EntityBase
	{
		[Key]
		public int Id { get; set; }
		
		[Timestamp]
		public byte[] TimeStamp { get; set; }			
	}
}
