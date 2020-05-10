using Services.Abstract;
using System;

namespace Services
{
	public static class PasswordCoding
	{
		public static string Decrypt(string password)
		{			
			var decryptedPass = "";
			foreach (char a in password)
			{
				char ch = a;
				ch--;
				decryptedPass += ch;
			}
			return decryptedPass;
		}

		public static string Encrypt(string password)
		{
			if (!string.IsNullOrEmpty(password))
			{
				var encryptedPass = "";
				foreach (char a in password)
				{
					char ch = a;
					ch++;
					encryptedPass += ch;
				}
				return encryptedPass;
			}
			else throw new ArgumentNullException($"Пароль пустой");
		}
	}
}
