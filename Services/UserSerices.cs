using Auction.Configurations;
using Auction.DTO;
using Auction.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Text;

namespace Auction.Services {
	public class UserSerices {

		//private readonly IConfiguration _configuration;

		//public UserSerices(IConfiguration configuration) {
		//	_configuration = configuration;
		//}

		public static async Task<string> login(UserLoginDTO user) {
				DbContext database = new DbContext();
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<UserData>("UserData");

				//var filter = Builders<UserData>.Filter.Eq(u => u.UserName.ToLower(), user.UserName.ToLower());
				//UserData userData = connection.Find(filter).FirstOrDefault();

				UserData userData = await connection.Find(u => u.UserEmail.ToLower().Equals(user.UserEmail)).FirstOrDefaultAsync();
				if (userData == null) {
					return "0";

				} else if (VerifyPasswordHash(user.UserPassword!, userData.PasswordHash, userData.PasswordSalt) == false) {

					return "0";
				} else {

					return CreateToken(userData);
				}
			
		}

		public static async Task<string> ResetPassword(string userEmail, string password) {

			if (await UserExists(userEmail)) {
				DbContext database = new DbContext();
				var connection = database.client.GetDatabase("AuctionApp").GetCollection<UserData>("UserData");

				var userData = await connection.Find(u => u.UserEmail.ToLower() == userEmail.ToLower()).FirstOrDefaultAsync();

				CreatePasswordHash(password, out byte[] PasswordHash, out byte[] PasswordSalt);
				userData.PasswordHash = PasswordHash;
				userData.PasswordSalt = PasswordSalt;

				connection.ReplaceOne(user => user._id.ToString() == userData._id.ToString(), userData);

				return "Password Updated";

			}

			return "UserEmail or Password Wrong";
		}
		public static async Task<string> ChangePassword(string userId, string oldPassword, string newPassword) {

			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<UserData>("UserData");


			var userData = await connection.Find(u => u._id.ToString() == userId).FirstOrDefaultAsync();

			if (VerifyPasswordHash(oldPassword, userData.PasswordHash, userData.PasswordSalt)) {

				CreatePasswordHash(newPassword, out byte[] PasswordHash, out byte[] PasswordSalt);
				userData.PasswordHash = PasswordHash;
				userData.PasswordSalt = PasswordSalt;

				connection.ReplaceOne(user => user._id.ToString() == userId, userData);

				return "Password Updated";
			}

			return "Incorrect Password !!";
		}
		public static async Task<string> Register(UserSignupDTO user) {

			if (await UserExists(user.UserEmail!)) {
				return "0";

			}
			CreatePasswordHash(user.ConfirmPassword!, out byte[] PasswordHash, out byte[] PasswordSalt);

			UserData userDB = new UserData() {
				UserName = user.UserName!,
				UserEmail = user.UserEmail!,
				PasswordSalt = PasswordSalt,
				PasswordHash = PasswordHash,
				Address = user.Address!,
			};
			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<UserData>("UserData");
			connection.InsertOne(userDB);

			return "1";
		}
		public static async Task<bool> UserExists(string userEmail) {
			DbContext database = new DbContext();
			var connection = database.client.GetDatabase("AuctionApp").GetCollection<UserData>("UserData");
			if ( (await connection.FindAsync(u => u.UserEmail.ToLower() == userEmail.ToLower())).FirstOrDefault() == null) {
				return false;
			}
			return true;
		}

		public static void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt) {

			using (var hmac = new System.Security.Cryptography.HMACSHA512()) {

				PasswordSalt = hmac.Key;
				PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
		public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {

			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {

				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

				return (computedHash.SequenceEqual(passwordHash));
			}
		}

		public static string CreateToken(UserData user) {

			// user information to store in the token
			var claims = new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, user._id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName)
			};


			//string appSettingsToken = "this is my custom Secret key for authentication kj sujfsd saddas asdas"; // string used to build hte user token
			string appSettingsToken = "MY_CUST0M_S3CR3T_Us3R_AuTH3NT!C@T!0N_T0K3N_F0R_MY_AUCT!0N_APP|!C@T!0N"; // string used to build hte user token

			//SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));

			SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


			// object used to create token
			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = creds
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			// making our token with the claims and objects
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

	}
}
