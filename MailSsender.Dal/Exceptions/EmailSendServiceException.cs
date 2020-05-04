using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MailSender.DAL.Exceptions
{
	public class EmailSendServiceException : Exception
    {
        public EmailSendServiceException()
        {
        }

        public EmailSendServiceException(string message)
            : base(message)
        {
        }

        public EmailSendServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected EmailSendServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
