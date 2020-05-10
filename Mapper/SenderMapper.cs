using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mapper
{
	public static class SenderMapper
	{
		public static SenderEntity ToEntity(this Sender sender)
		{
			return new SenderEntity()
			{
				Id = sender.Id,
				Email = sender.Email,
				Password = sender.Password
			};
		}
		public static Sender ToDomain(this SenderEntity sender)
		{
			return new Sender()
			{
				Id = sender.Id,
				Email = sender.Email,
				Password = sender.Password
			};
		}
	}
}
