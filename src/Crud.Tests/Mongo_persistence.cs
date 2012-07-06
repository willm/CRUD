using System;
using NUnit.Framework;
using CRUD;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace Crud.Tests
{
	[TestFixture]
	public class Mongo_persistence
	{
		MongoRepository _subject = new MongoRepository();
		MongoServer _server;
		MongoDatabase _tracksDb;
		MongoCollection _tracks;
		MongoTrack _track = new MongoTrack(){Artist = "Squarepusher", RunningTime = 5.2, Title = "Planetarium"};
		
		[SetUp]
		public void Setup()
		{
			_server = MongoServer.Create("mongodb://localhost");
			_tracksDb = _server.GetDatabase("tracksdb");
			_tracks = _tracksDb.GetCollection<MongoTrack>("tracks");
			ClearData();
		}
		
		[TearDown]
		public void ClearData()
		{
			_tracks.RemoveAll();
		}
		
		[Test]
		public void can_save_track()
		{
			_subject.SaveOrUpdate(_track);
			
			MongoTrack resultTrack = GetTrackById(_track._id);
			
			Assert.That(resultTrack.Artist, Is.EqualTo("Squarepusher"));
			Assert.That(resultTrack.Title, Is.EqualTo("Planetarium"));
			Assert.That(resultTrack.RunningTime, Is.EqualTo(5.2));
		}
		
		[Test]
		public void can_get_track_by_artist_and_title()
		{
			_tracks.Save(new MongoTrack{Artist = "Squarepusher", Title = "Red Hot Car"});
			_tracks.Save(_track);
			_tracks.Save(new MongoTrack{Artist = "Squarepusher", Title = "Hello Meow"});
			
			MongoTrack actual = _subject.GetByArtistAndTitle("Squarepusher", "Planetarium");
			
			Assert.That(actual.Title, Is.EqualTo("Planetarium"));
		}
		
		[Test]
		public void can_delete_track()
		{
			can_save_track();
			_subject.Delete(_track);
			
			Assert.That (GetTrackById(_track._id), Is.Null);
		}
		
		[Test]
		public void can_update_track()
		{
			can_save_track();
			_track.Title = "Something Else";
			_subject.SaveOrUpdate(_track);
			
			Assert.That(GetTrackById(_track._id).Title, Is.EqualTo("Something Else"));
		}
		
		MongoTrack GetTrackById(ObjectId trackId){
			MongoTrack track;
			using (_server.RequestStart(_tracksDb)){
				track = _tracks.FindOneByIdAs<MongoTrack>(trackId);
			}
			return track;
		}
		
	}
}

