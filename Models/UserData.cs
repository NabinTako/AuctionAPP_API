using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Auction.Models {
	public class UserData {
		
		[BsonId]
		public ObjectId _id { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public byte[] PasswordHash { get; set; } = new byte[0];
		public byte[] PasswordSalt { get; set; } = new byte[0];
		public string Address { get; set; }
	}
}
