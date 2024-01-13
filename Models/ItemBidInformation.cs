using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Auction.Models {
	public class ItemBidInformation {

		[BsonId]
		public ObjectId _id { get; set; }
		public string UserId { get; set; } = null;
		public string ItemId { get; set; } = null;
		public string BitAmount { get; set; } = null;
	}
}
