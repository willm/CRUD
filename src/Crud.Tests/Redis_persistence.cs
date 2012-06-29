using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookSleeve;
using NUnit.Framework;

namespace Crud.Tests
{
	[TestFixture]
	public class Redis_persistence
	{
		[Test]
		public void can_save()
		{
			using(var connection = new RedisConnection("localhost")){
				connection.Open();
				connection.Strings.Set(1, "title", "bobby's tune");
			}
			
			using(var connection = new RedisConnection("localhost")){
				var result = connection.Strings.Get (1, "title");
				Assert.That (result, Is.EqualTo("bobby's tune"));
			}
		}
	}
}
