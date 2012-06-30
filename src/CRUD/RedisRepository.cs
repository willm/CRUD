using System;
using BookSleeve;

namespace CRUD
{
	public class RedisRepository
	{
		private RedisConnection _connection;
		
		public RedisRepository (RedisConnection connection)
		{
			_connection = connection;
		}
		
		private static int DB = 1;
		
		public void Save(string key, string value){
				_connection.Strings.Set(DB, key, value);
		}
		
		public string Get(string key){
			var result = _connection.Strings.GetString(DB, key);
			return _connection.Wait(result);
		}
		
		public void Delete(string key){
			_connection.Keys.Remove(DB, key);
		}
	}
}

