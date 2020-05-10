using System;
using System.Collections.Generic;
using Domain;

namespace Services.Abstract
{
	public interface ISenderService
	{
		void Add(Sender recipient);
		IEnumerable<Sender> GetAll();
		int Edit(Sender recipient);
		int Delete(Sender recipient);
	}
}
