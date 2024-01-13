using Auction.Configurations;
using Auction.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Auction.Services;
using Auction.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Auction.Controllers {

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {


		[AllowAnonymous]
		[HttpPost("signup")]
		public async Task<ActionResult<string>> UserSignUp(UserSignupDTO userData) {
			try {
				if (await UserSerices.Register(userData) == "1") {

					return "UserSignUp success";
				} else {
					return "User with this email exists";
				}
			}catch (Exception ex) {
				return ex.Message;
			}

		}

		[AllowAnonymous]
		[HttpPost("signin")]
		public async Task<ActionResult<string>> UserLoginIn(UserLoginDTO userData) {
			try {
				var response = await UserSerices.login(userData);
				if (response != "0") {

					return response.ToString()!;
				} else {
					return "UserEmail or password doesnot match!";
				}
			}catch(Exception ex) {
				return ex.Message;
			}

		}

		[AllowAnonymous]
		[HttpPost("forgotpassword")]
		public async Task<ActionResult<string>> ResetPassword(string useremail, string password) {
			try {
				var response = await UserSerices.ResetPassword(useremail, password);
				if (response != "0") {

					return response.ToString()!;
				} else {
					return "Username or password doesnot match!";
				}
			}catch (Exception ex) {
				return ex.Message;
			}

		}

		[HttpPost("changepassword")]
		public async Task<ActionResult<string>> ChangePassword(string oldPassword,string newPassword) {
			try { 
			string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

			var response = await UserSerices.ChangePassword(userId, oldPassword, newPassword);
			if (response != "0") {

				return response.ToString()!;
			} else {
				return "Username or password doesnot match!";
			}
			} catch (Exception ex) {
				return ex.Message;
			}

		}
	}
}
