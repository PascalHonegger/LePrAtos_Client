// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Security.Cryptography;

namespace LePrAtos.Startup
{
	/// <summary>
	///     Class used to hash Passwords
	/// </summary>
	public class PasswordHasher
	{
		/// <summary>
		///     Hash a password
		/// </summary>
		/// <param name="originalPassword">Password to be hashed</param>
		/// <returns>Hashed password</returns>
		public static string HashPasswort(string originalPassword)
		{
			return originalPassword;
		}
	}
}