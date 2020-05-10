using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mapper
{
	public static class RecipientMapper
	{
		public static RecipientEntity ToEntity(this Recipient recipient)
		{
			return new RecipientEntity()
			{
				Id = recipient.Id,
				Name = recipient.Name,
				Email = recipient.Email
			};
		}
		public static Recipient ToDomain(this RecipientEntity recipient)
		{
			return new Recipient()
			{
				Id = recipient.Id,
				Name = recipient.Name,
				Email = recipient.Email
			};
		}
	}
}
