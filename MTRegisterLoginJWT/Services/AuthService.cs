﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.DTO;
using MTRegisterLoginJWT.Data.Models;
using MTRegisterLoginJWT.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MTRegisterLoginJWT.Services
{
	public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		public AuthService(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<int>> Register(UserRegisterDTO model)
		{
			var response = new ServiceResponse<int>();

			if(await UserExists(model.UserName))
			{
				response.Success = false;
				response.Message = "User already exists";
				return response;
			}

			CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

			var user = new User()
			{
				UserName = model.UserName,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};

			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();

			response.Data = user.Id;
			return response;
		}

		public async Task<ServiceResponse<string>> Login(UserLoginDTO model)
		{
			var response = new ServiceResponse<string>();

			var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == model.UserName.ToLower());

			if (user == null)
			{
				response.Success = false;
				response.Message = "User not found";
			}
			else if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
			{
				response.Success = false;
				response.Message = "Wrong password";
			}
			else
			{
				response.Data = GenerateAccessToken(user);
			}

			return response;
		}

		private async Task<bool> UserExists(string userName)
		{
			if(await _db.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
			{
				return true;
			}
			return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using(var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computeHash.SequenceEqual(passwordHash);
			}
		}

		private string GenerateAccessToken(User user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.UserName)
			};

			SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
				.GetBytes(_configuration.GetSection("Token:Secret").Value));

			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = credentials
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

	}
}
