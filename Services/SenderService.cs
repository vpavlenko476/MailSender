using Data.Repositories.Abstract;
using Domain;
using Entities;
using Mapper;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
	public class SenderService : ISenderService
	{
		private readonly IBaseRepo<SenderEntity> _repository;
		public SenderService(IBaseRepo<SenderEntity> repository)
		{
			_repository = repository;
		}
		public void Add(Sender sender)
		{
			_repository.Add(sender.ToEntity());
		}

		public int Delete(Sender sender)
		{
			return _repository.Delete(sender.ToEntity());
		}

		public int Edit(Sender sender)
		{
			return _repository.Save(sender.ToEntity());
		}

		public IEnumerable<Sender> GetAll()
		{
			return _repository.GetAll().Select(x => x.ToDomain());
		}
	}
}
