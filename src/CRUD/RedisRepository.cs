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
		
		private const int DB = 1;
				
		public void SaveOrUpdate(Track track){
			_connection.Hashes.Set(DB, track.Id.ToString(), track.ToByteArrayDictionary());
		}
		
		public void Delete(Track track){
			_connection.Keys.Remove(DB,track.Id.ToString());
		}
		
		public Track Get(Guid trackId){
			var getResult =_connection.Hashes.GetAll(DB, trackId.ToString());
			var hashResult = _connection.Wait(getResult);
			return Track.FromHash(trackId, hashResult);
		}
	}
}

