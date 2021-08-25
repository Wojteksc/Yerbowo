using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Yerbowo.Domain.Addresses;
using Yerbowo.Domain.SeedWork;

namespace Yerbowo.Domain.Users
{
	public class User : BaseEntity
	{
		public string FirstName { get; protected set; }
		public string LastName { get; protected set; }
		public string CompanyName { get; protected set; }
		public string Email { get; protected set; }
		public string Role { get; protected set; }
		public string PhotoUrl { get; protected set; }
		public string Provider { get; protected set; }
		public byte[] PasswordHash { get; protected set; }
		public byte[] PasswordSalt { get; protected set; }

		public List<Address> Addresses { get; protected set; }

		private User() { }

		public User(string firstName, string lastName, string email,
			string companyName, string role, string photoUrl,
			string provider, string password)
		{

			Guard.Against.NullOrEmpty(firstName, nameof(firstName));
			Guard.Against.NullOrEmpty(lastName, nameof(lastName));
			Guard.Against.NullOrEmpty(email, nameof(email));
			Guard.Against.NullOrEmpty(password, nameof(password));

			FirstName = firstName;
			LastName = lastName;
			Email = email;
			CompanyName = companyName;
			SetRole(role);
			PhotoUrl = photoUrl;
			Provider = provider;
			SetPassword(password);
		}

		public void SetPassword(string passwordToHash)
		{
			Guard.Against.NullOrEmpty(passwordToHash, nameof(passwordToHash));

			using (var hmac = new HMACSHA512())
			{
				PasswordSalt = hmac.Key;
				PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordToHash));
			}
		}

		public void SetRole(string role)
		{
			Guard.Against.NullOrEmpty(role, nameof(role));

			Role = role;
		}

		public void SetPhotoUrl(string photoUrl)
		{
			Guard.Against.NullOrEmpty(photoUrl, nameof(photoUrl));

			PhotoUrl = photoUrl;
		}
	}
}
