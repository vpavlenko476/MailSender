using MailSender.DAL.Models.Base;
using MailSender.DAL.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace MailSender.DAL.Repos
{
	public class BaseRepo<T> : IRepo<T>, IDisposable
		where T : EntityBase, new()
	{
		private readonly MailSenderContext _db;
		private readonly DbSet<T> _table;
		protected MailSenderContext Context => _db;
		public BaseRepo()
		{
			_db = new MailSenderContext();
			_table = _db.Set<T>();

		}
		public int Add(T entity)
		{
			_table.Add(entity);
			return _db.SaveChanges();
		}

		public int AddRandge(IList<T> entites)
		{
			_table.AddRange(entites);
			return _db.SaveChanges();
		}

		public int Delete(T entity)
		{
			_db.Entry(entity).State = EntityState.Deleted;
			return _db.SaveChanges();
		}		

		public List<T> GetAll()
		{
			return _table.ToList();
		}

		public T GetOne(int? Id)
		{
			return _table.Find(Id);
		}

		public int Save(T entity)
		{
			_db.Entry(entity).State = EntityState.Modified;
			return _db.SaveChanges();
		}

		public void Dispose()
		{
			_db.Dispose();
		}
	}
}
