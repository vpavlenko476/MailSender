using Domain;
using Entities;

namespace Mapper
{
	public static class HostMapper
	{
		public static HostEntity ToEntity(this Host host)
		{
			return new HostEntity()
			{
				Id = host.Id,
				Server = host.Server,
				Port = host.Port
			};
		}
		public static Host ToDomain(this HostEntity host)
		{
			return new Host()
			{
				Id = host.Id,
				Server = host.Server,
				Port = host.Port
			};
		}
	}
}
