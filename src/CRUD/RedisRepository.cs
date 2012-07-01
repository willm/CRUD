using System;
using BookSleeve;

namespace CRUD
{
	public class RedisRepository : IRepository
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
		
		public void Save(Track track){
			_connection.Hashes.Set(DB, track.Id.ToString(), track.ToByteArrayDictionary());
		}
		
		public void Delete(Track track){
			_connection.Keys.Remove(DB,track.Id.ToString());
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

