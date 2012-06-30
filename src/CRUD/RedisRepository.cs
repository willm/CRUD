using System;
using BookSleeve;

namespace CRUD
{
	public class RedisRepository
	{
		private static int DB = 1;
		
		public void Save(string key, string value, RedisConnection connection){
			//using(var connection = new RedisConnection("localhost")){
				//connection.Open();
				connection.Strings.Set(DB, key, value);
			//}
		}
	}
}

