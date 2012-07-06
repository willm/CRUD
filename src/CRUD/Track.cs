using System;
using System.Text;
using System.Collections.Generic;
using MongoDB.Bson;

namespace CRUD
{
	public class Track : ITrack
	{
		public Track(){
			Id = Guid.NewGuid();
		}
		public string Title { get; set; }
		public string Artist { get; set; }
		public double RunningTime { get;set;}
		public Guid Id{get; private set;}
		
		public Dictionary<string,Byte[]> ToByteArrayDictionary(){
			//This is neccessary to save as reddis hash
			var trackDictionary = new Dictionary<string, Byte[]>();
			trackDictionary.Add("Title", System.Text.Encoding.UTF8.GetBytes(Title));
			trackDictionary.Add("Artist", System.Text.Encoding.UTF8.GetBytes(Artist));
			trackDictionary.Add("Running Time", System.Text.Encoding.UTF8.GetBytes(RunningTime.ToString()));
			return trackDictionary;
		}
		public static Track FromHash(Guid id, Dictionary<string, Byte[]> hash){
			return new Track(){
				Id = id, 
				Artist = Encoding.UTF8.GetString(hash["Artist"]),
				Title = Encoding.UTF8.GetString(hash["Title"]),
				RunningTime = Double.Parse(Encoding.UTF8.GetString(hash["Running Time"]))			
			};
		}
	}
	
	public class MongoTrack : ITrack
	{
		public ObjectId _id {get; set;}
		public string Title { get; set; }
		public string Artist { get; set; }
		public double RunningTime { get;set;}
	}
	
	public interface ITrack
	{
		string Title { get; set; }
		string Artist { get; set; }
		double RunningTime { get;set;}
	}
}
