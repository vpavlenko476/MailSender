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
	public class RecipientService : IRecipientService
	{
		private readonly IBaseRepo<RecipientEntity> _repository;
		public RecipientService(IBaseRepo<RecipientEntity> repository)
		{
			_repository = repository;
		}
		public void Add(Recipient recipient)
		{
			_repository.Add(recipient.ToEntity());
		}

		public int Delete(Recipient recipient)
		{
			return _repository.Delete(recipient.ToEntity());
		}

		public int Edit(Recipient recipient)
		{
			return _repository.Save(recipient.ToEntity());
		}

		public IEnumerable<Recipient> GetAll()
		{
			return _repository.GetAll().Select(x => x.ToDomain());
		}		
	}
}
