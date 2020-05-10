using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Abstract
{
	public interface IRecipientService
	{
		void Add(Recipient recipient);
		IEnumerable<Recipient> GetAll ();
		int Edit(Recipient recipient);
		int Delete(Recipient recipient);
	}
}
