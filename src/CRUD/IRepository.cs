using System;

namespace CRUD
{
	public interface IRepository
	{
		void SaveOrUpdate(Track track);
		Track Get(Guid trackId);
		void Delete(Track track);
	}
}

