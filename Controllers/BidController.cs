using Auction.DTO;
using Auction.Models;
using Auction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auction.Controllers {

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class BidController : ControllerBase {

		[AllowAnonymous]
		[HttpGet("getbidinfo")]
		public async Task<ActionResult<List<BidInfoDTO>>> GetBidInfoForUsers() {
			try {
				var response = await ItemServices.GetBidInformation();

				return Ok(response);
			}catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("getyourbidinfo")]
		public async Task<ActionResult<List<BidInfoDTO>>> GetBidInfoByUsers() {
			try {
				string userId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)!.Value;

				var response = await ItemServices.GetBidInformationByUser(userId);

				return Ok(response);
			}catch(Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("registeryouritembid")]
		public async Task<ActionResult<string>> RegesterItemBid(UpdateBidDTO itemData) {

			try {
				string userId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)!.Value;

				await ItemServices.RegesterBid(userId,itemData);

				return Ok("Database updated");
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}

		}

		[HttpPost("updateyouritembidinfo")]
		public async Task<ActionResult<string>> UpdateBidAmount(UpdateBidDTO updatedAmountData) {
			try {
				string userId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)!.Value;

				var sucess = await ItemServices.UpdateBidInfo(userId, updatedAmountData);
				if(sucess == false) { return "item to update Not found!!"; }
				return "Updated";

			}catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

	}
}
