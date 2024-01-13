using Auction.Configurations;
using Auction.DTO;
using Auction.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace Auction.Services {
	public class ItemServices {
		public static async Task<List<BidInfoDTO>> GetBidInformation() {

			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<ItemBidInformation>("BidInformation");
			var DbBidInfo = (await connection.FindAsync(u => true)).ToList();

			List<BidInfoDTO> response = new List<BidInfoDTO>();

			foreach (var data in DbBidInfo) {
				response.Add(new BidInfoDTO { UserId = data.UserId, ItemId = data.ItemId, BitAmount = data.BitAmount });
			}
			return response;

		}

		public static async Task<List<BidInfoDTO>> GetBidInformationByUser(string userId) {

			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<ItemBidInformation>("BidInformation");
			//string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
			var DbBidInfo = (await connection.FindAsync(u => u.UserId == userId)).ToList();

			List<BidInfoDTO> response = new List<BidInfoDTO>();

			foreach (var data in DbBidInfo) {
				response.Add(new BidInfoDTO { UserId = data.UserId, ItemId = data.ItemId, BitAmount = data.BitAmount });
			}
			return response;

		}
		public static async Task RegesterBid(string userId,UpdateBidDTO itemData) {
			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<ItemBidInformation>("BidInformation");

			ItemBidInformation mappedItemBidData = new ItemBidInformation() {
				UserId = userId,
				ItemId = itemData.ItemId,
				BitAmount = itemData.BitAmount
			};

			connection.InsertOne(mappedItemBidData);

		}

		public static async Task<bool> UpdateBidInfo(string userId, UpdateBidDTO updatedBid) {

			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<ItemBidInformation>("BidInformation");

			var DbBidInfo = (await connection.FindAsync(u => u.UserId == userId && u.ItemId == updatedBid.ItemId)).FirstOrDefault();

			if(DbBidInfo == null) { return false; }

			ItemBidInformation mappedItemData = new ItemBidInformation() {
				_id = DbBidInfo._id,
				UserId = DbBidInfo.UserId,
				ItemId = updatedBid.ItemId,
				BitAmount = updatedBid.BitAmount
			};

			connection.ReplaceOne(BidInfo => BidInfo._id.ToString() == DbBidInfo._id.ToString(), mappedItemData);

			return true;
		}
	}
}
