using MongoDB.Driver;

namespace Auction.Configurations {
	public class DbContext {

		public MongoClient client;
		public DbContext() {
			client = new MongoClient("mongodb+srv://nabintako999:EHXJiuTRqq4qLWYx@cluster0.wdaqh4h.mongodb.net/?retryWrites=true&w=majority");

		}

		


	}
}
