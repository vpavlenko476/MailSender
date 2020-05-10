using Data.Repositories.Abstract;
using Domain;
using Entities;
using Mapper;
using Services.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
	public class HostService : IHostService
	{
		private readonly IBaseRepo<HostEntity> _repository;
		public HostService(IBaseRepo<HostEntity> repository)
		{
			_repository = repository;
		}
		public void Add(Host host)
		{
			_repository.Add(host.ToEntity());
		}

		public int Delete(Host host)
		{
			return _repository.Delete(host.ToEntity());
		}

		public int Edit(Host host)
		{
			return _repository.Save(host.ToEntity());
		}

		public IEnumerable<Host> GetAll()
		{
			return _repository.GetAll().Select(x => x.ToDomain());
		}
	}
}

