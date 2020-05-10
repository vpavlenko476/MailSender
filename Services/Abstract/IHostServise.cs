using System;
using System.Collections.Generic;
using Domain;

namespace Services.Abstract
{
	public interface IHostService
	{
		void Add(Host recipient);
		IEnumerable<Host> GetAll();
		int Edit(Host recipient);
		int Delete(Host recipient);
	}
}
