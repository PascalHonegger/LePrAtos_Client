// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LePrAtos.Startup
{
	/// <summary>
	///     Class used to hash Passwords
	/// </summary>
	public static class PasswordHasher
	{
		/// <summary>
		///     Hash a password
		/// </summary>
		/// <param name="originalPassword">Password to be hashed</param>
		/// <returns>Hashed password</returns>
		public static string HashPasswort(string originalPassword)
		{
			return GetHashString(originalPassword);
		}

		private static IEnumerable<byte> GetHash(string inputString)
		{
			HashAlgorithm algorithm = SHA256.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString ?? string.Empty));
		}

		private static string GetHashString(string inputString)
		{
			var sb = new StringBuilder();
			foreach (var b in GetHash(inputString))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}
	}
}