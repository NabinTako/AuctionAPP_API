using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Auction.Models {
	public class ItemAdditionalData {

		[BsonId]
		public ObjectId _id { get; set; }

		public ObjectId ItemId { get; set; }
	}
}
