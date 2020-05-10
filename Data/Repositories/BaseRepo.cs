using Data.Context;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Data.Repositories.Abstract
{
	/// <summary>
	/// CRUD Repository
	/// </summary>
	/// <typeparam name="T">EntityModel</typeparam>
	public class BaseRepo<T> : IBaseRepo<T>
		where T : EntityBase, new()
	{
		private MailSenderContext _db { get; }
		private readonly DbSet<T> _table;
		protected MailSenderContext Context => _db;
		public BaseRepo(MailSenderContext context)
		{
			if (context == null) throw new ArgumentNullException("Null DbContext");
			_db = context;
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
			var _entity = _table.Find(entity.Id);
			_db.Entry(_entity).State = EntityState.Deleted;
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
			var _entity = _table.Find(entity.Id);
			_db.Entry(_entity).CurrentValues.SetValues(entity);			
			return _db.SaveChanges();
		}
		public void Dispose()
		{
			_db.Dispose();
		}
	}
}
