using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;



namespace Auction.Models {
	public class AuctionItem {

		[BsonId]
		public ObjectId _id { get; set; }
		public string? ImageUrl { get; set; } = null;
		public string? ArtistName { get; set; } = null;

		public string? Classification { get; set; } = null;

		public string? Detail { get; set; } = null;

		public string? EstimatedPrice { get; set; }

		public string? ItemType { get; set; } = null;

		public string? Dimensions { get; set; } = null;

		public string? Medium { get; set; } = null;

		public string? Weight { get; set; } = null;

		public string? IsFramed { get; set; } = null;



	}
}
