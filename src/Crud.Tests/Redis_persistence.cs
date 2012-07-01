using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookSleeve;
using NUnit.Framework;
using CRUD;

namespace Crud.Tests
{
	[TestFixture]
	public class Redis_persistence
	{
		string expectedString = "bobby's tune";
		string key = "title";
		int db = 1;
		IRepository subject;
		RedisConnection connection;
		
		[SetUp]
		[TearDown]
		public void ClearData(){
			connection = new RedisConnection("localhost", 6379, -1, null, 2147483647, true,10000);
			subject = new RedisRepository(connection);
			connection.Open();
			connection.Server.FlushDb(db);
		}

		[Test]
		public void can_save_track(){
			Track track = new Track(){Artist = "Squarepusher", RunningTime = 5.2, Title = "Planetarium"};
			subject.SaveOrUpdate(track);
			
			Track resultTrack = GetTrackById(track.Id);
			
			Assert.That(resultTrack.Artist, Is.EqualTo("Squarepusher"));
			Assert.That(resultTrack.Title, Is.EqualTo("Planetarium"));
			Assert.That(resultTrack.RunningTime, Is.EqualTo(5.2));
		}
		
		[Test]
		public void can_update_track(){
			Track initialTrack = new Track(){Artist = "Squarepusher", RunningTime = 5.2, Title = "Planetarium"};
			subject.SaveOrUpdate(initialTrack);
			
			Track resultTrack = GetTrackById(initialTrack.Id);
			
			Assert.That(resultTrack.Artist, Is.EqualTo("Squarepusher"));
			Assert.That(resultTrack.Title, Is.EqualTo("Planetarium"));
			Assert.That(resultTrack.RunningTime, Is.EqualTo(5.2));
			
			resultTrack.Artist = "Aphex";
			subject.SaveOrUpdate(resultTrack);
			
			var updatedTrack = GetTrackById(initialTrack.Id);
			
			Assert.That(updatedTrack.Artist, Is.EqualTo("Aphex"));
			Assert.That(updatedTrack.Title, Is.EqualTo("Planetarium"));
			Assert.That(updatedTrack.RunningTime, Is.EqualTo(5.2));
		}
		
		public Track GetTrackById(Guid trackId){
			var result = connection.Hashes.GetAll(db, trackId.ToString());
			Dictionary<string, Byte[]> resultsDic = connection.Wait(result);
			return Track.FromHash(trackId,resultsDic);
		}
			
		[Test]
		public void should_get_Track(){
			Track track = new Track(){Artist = "Squarepusher", RunningTime = 5.2, Title = "Planetarium"};
			subject.SaveOrUpdate(track);
			
			Track resultTrack = subject.Get(track.Id);
			
			Assert.That(resultTrack.Artist, Is.EqualTo("Squarepusher"));
			Assert.That(resultTrack.Title, Is.EqualTo("Planetarium"));
			Assert.That(resultTrack.RunningTime, Is.EqualTo(5.2));
		}
		
		[Test]
		public void can_delete_track(){
			Track track = new Track{Artist = "Squarepusher", RunningTime = 5.2, Title = "Planetarium"};
			subject.SaveOrUpdate(track);
			
			subject.Delete(track);
			
			Assert.Throws<KeyNotFoundException>(()=>GetTrackById(track.Id));
		}
		
	}
}
