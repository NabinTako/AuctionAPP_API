using Auction.Configurations;
using Auction.DTO;
using Auction.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Auction.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuctionItemsController : ControllerBase {

		DbContext database = new DbContext();

		[HttpGet("getdata")]
		public ActionResult<AuctionItemGetDTO> GetItemData() {
			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");
				List<AuctionItem> data = connection.Find(auctionItem => true).ToList();
				List<AuctionItemGetDTO> response = new List<AuctionItemGetDTO>();
				foreach (var itemdata in data) {
					response.Add(new AuctionItemGetDTO {
						_id = itemdata._id.ToString(),
						ImageUrl = itemdata.ImageUrl,
						ArtistName = itemdata.ArtistName,
						Classification = itemdata.Classification,
						Detail = itemdata.Detail,
						EstimatedPrice = itemdata.EstimatedPrice,
						ItemType = itemdata.ItemType,
						Dimensions = itemdata.Dimensions,
						Medium = itemdata.Medium,
						Weight = itemdata.Weight,
						IsFramed = itemdata.IsFramed,
					});
				}

				return Ok(response);
			}catch (Exception ex) {
				return (new AuctionItemGetDTO());
			}

		}

		[HttpGet("getsingledata/{id}")]
		public ActionResult<AuctionItemGetDTO> GetSingleItemData(string id) {
			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");

				List<AuctionItem> data = connection.Find(auctionItem => auctionItem._id.ToString() == id).ToList();
				AuctionItemGetDTO response = new AuctionItemGetDTO {
					_id = data[0]._id.ToString(),
					ImageUrl = data[0].ImageUrl,
					ArtistName = data[0].ArtistName,
					Classification = data[0].Classification,
					Detail = data[0].Detail,
					EstimatedPrice = data[0].EstimatedPrice,
					ItemType = data[0].ItemType,
					Dimensions = data[0].Dimensions,
					Medium = data[0].Medium,
					Weight = data[0].Weight,
					IsFramed = data[0].IsFramed,
				};
				return Ok(response);
			}catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("filterbyname/{type}")]
		public ActionResult<AuctionItemGetDTO> GetItemDataThroughClassification(string type) {
			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");

				List<AuctionItem> data = connection.Find(auctionItem => auctionItem.Classification == type.Trim()).ToList();
				List<AuctionItemGetDTO> response = new List<AuctionItemGetDTO>();

				foreach (var item in data) {
					response.Add(new AuctionItemGetDTO {
						_id = item._id.ToString(),
						ImageUrl = item.ImageUrl,
						ArtistName = item.ArtistName,
						Classification = item.Classification,
						Detail = item.Detail,
						EstimatedPrice = item.EstimatedPrice,
						ItemType = item.ItemType,
						Dimensions = item.Dimensions,
						Medium = item.Medium,
						Weight = item.Weight,
						IsFramed = item.IsFramed
					});
				}
				return Ok(response);
			}catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}


		[HttpPost("updateitem/{id}")]
		public ActionResult<string> UpdateItems(string id, AuctionItemDTO itemData ) {

			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");

				List<AuctionItem> data = connection.Find(auctionItem => auctionItem._id.ToString() == id).ToList();

				AuctionItem mappedItemData = new AuctionItem() {
					_id = data[0]._id,
					ImageUrl = itemData.ImageUrl,
					ArtistName = itemData.ArtistName,
					Classification = itemData.Classification,
					Detail = itemData.Detail,
					EstimatedPrice = itemData.EstimatedPrice,
					ItemType = itemData.ItemType,
					Dimensions = itemData.Dimensions,
					Medium = itemData.Medium,
					Weight = itemData.Weight,
					IsFramed = itemData.IsFramed,
				};

				connection.ReplaceOne(auctionItem => auctionItem._id.ToString() == id, mappedItemData);

				return Ok("Database updated");
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}

		}


		[HttpPost("registeritem")]
		public ActionResult<string> RegesterItems(AuctionItemDTO itemData) {

			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");

				AuctionItem mappedItemData = new AuctionItem() {
					ArtistName = itemData.ArtistName,
					ImageUrl = itemData.ImageUrl,
					Classification = itemData.Classification,
					Detail = itemData.Detail,
					EstimatedPrice = itemData.EstimatedPrice,
					ItemType = itemData.ItemType,
					Dimensions = itemData.Dimensions,
					Medium = itemData.Medium,
					Weight = itemData.Weight,
					IsFramed = itemData.IsFramed,
				};

				connection.InsertOne(mappedItemData);

				return Ok("Database updated");
			}catch (Exception ex) {
				return BadRequest(ex.Message);
			}

		}

		[HttpDelete("deleteitem/{id}")]
		public ActionResult<string> DeleteItem(string id) {

			try {
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<AuctionItem>("ItemData");

				connection.DeleteOne(auctionItem => auctionItem._id.ToString() == id);

				return Ok("Database updated");
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}

		}


	}
}
