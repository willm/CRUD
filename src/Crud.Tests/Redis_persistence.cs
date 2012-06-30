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
		RedisRepository subject;
		RedisConnection connection;
		
		[SetUp]
		[TearDown]
		public void ClearData(){
			connection = new RedisConnection("localhost");
			subject = new RedisRepository(connection);
				connection.Open();
				connection.Keys.Remove(db, key).Wait(2);
		}		
		
		[Test]
		public void can_save()
		{			
			subject.Save(key,expectedString);	
			var result = connection.Strings.GetString (db, key);
			
			Assert.That (connection.Wait(result), Is.EqualTo(expectedString));
		}
		
		[Test]
		public void can_update()
		{			
			connection.Strings.Set(db, key, expectedString);
			
			subject.Save(key, "new value");
			
			var result = connection.Strings.GetString (db, key);
			Assert.That (connection.Wait(result), Is.EqualTo("new value"));
		}
		
		[Test]
		public void should_get(){
			connection.Strings.Set(db, key, expectedString);
			
			var result = subject.Get(key);
			
			Assert.That (result, Is.EqualTo(expectedString));
		}
		
		[Test]
		public void can_delete(){
			connection.Strings.Set(db, key, expectedString);
			
			var result = connection.Strings.GetString(db,key);
			Assert.That (connection.Wait(result), Is.EqualTo(expectedString));
	
			subject.Delete(key);
			
			var result2 = connection.Strings.GetString(db,key);
			Assert.That (connection.Wait(result2), Is.EqualTo(null));
		}
	}
}
