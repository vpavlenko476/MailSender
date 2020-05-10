using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mapper
{
	public static class MessageMapper
	{
		public static MessageEntity ToEntity(this Message message)
		{
			return new MessageEntity()
			{
				Id = message.Id,
				SenderId = message.SenderId,
				Body = message.Body,
				Title = message.Title,
				ScheduledSendDateTime = message.ScheduledSendDateTime,
				SendDateTime = message.SendDateTime
			};
		}
		public static Message ToDomain(this MessageEntity message)
		{
			return new Message()
			{
				Id = message.Id,
				SenderId = message.SenderId,
				Body = message.Body,
				Title = message.Title,
				ScheduledSendDateTime = message.ScheduledSendDateTime,
				SendDateTime = message.SendDateTime
			};
		}
	}
}
