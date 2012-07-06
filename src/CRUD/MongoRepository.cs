using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace CRUD
{
	public class MongoRepository
	{
		MongoServer _server = MongoServer.Create("mongodb://localhost");
		MongoDatabase _testDB;
		MongoCollection _tracks;
		
		public MongoRepository ()
		{
			_testDB = _server.GetDatabase("tracksdb");
			_tracks = _testDB.GetCollection<MongoTrack>("tracks");
		}
		
		public void SaveOrUpdate(MongoTrack track)
		{
			_tracks.Save(track);
		}
		
		public MongoTrack GetByArtistAndTitle(string artist, string title)
		{
			var query = Query.And(
				Query.EQ("Artist", artist),
				Query.EQ("Title", title)
				);
			return _tracks.FindOneAs<MongoTrack>(query);
			//return results.AsQueryable<MongoTrack>().SingleOrDefault(t => t.Title == "Planetarium");
		}
			
		public void Delete(MongoTrack track)
		{
			_tracks.Remove(Query.EQ("_id",track._id));
		}
	}
}

