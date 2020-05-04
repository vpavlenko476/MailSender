using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.DAL.Repos
{
	interface IBaseRepo<T>
	{
		int Add(T entity);
		int AddRandge(IList<T> entites);
		int Save(T entite);
		int Delete(T entity);
		T GetOne(int? Id);
		List<T> GetAll();
	}
}
