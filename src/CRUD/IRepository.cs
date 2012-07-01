using System;

namespace CRUD
{
	public interface IRepository
	{
		void Save(string key, string value);
		void Save(Track track);
		void Delete(string key);
		string Get(string key);
	}
}

