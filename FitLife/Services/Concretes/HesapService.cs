using System.Security.Cryptography;
using System.Text;
using FitLife.Services.Abstracts;

namespace FitLife.Services.Concretes
{
	public class HesapService : IHesapService
	{
		public string HashPassword(string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

				// Hash'i bir string olarak döndürmek için hex formatına çevirme
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < hashedBytes.Length; i++)
				{
					stringBuilder.Append(hashedBytes[i].ToString("x2"));
				}

				return stringBuilder.ToString();
			}
		}
	}
}
