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
		RedisRepository subject = new RedisRepository();
		
		[SetUp]
		[TearDown]
		public void ClearData(){
			using(var con = new RedisConnection("localhost")){
				con.Open();
				con.Keys.Remove(db, key).Wait(2);
			}
		}		
		
		[Test]
		public void can_save()
		{			
			using(var connection = new RedisConnection("localhost")){
				connection.Open();
				subject.Save(key,expectedString, connection);	
				var result = connection.Strings.GetString (db, key);
				
				Assert.That (connection.Wait(result), Is.EqualTo(expectedString));
			}
		}
	}
}
