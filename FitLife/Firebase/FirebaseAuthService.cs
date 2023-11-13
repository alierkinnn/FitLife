

using Firebase.Auth;

namespace FitLife.Firebase
{
	public class FirebaseAuthService
	{
		public async Task<string> SignInWithEmailAndPasswordAsync(string email, string password)
		{
			try
			{
				var auth = FirebaseAuth.DefaultInstance;
				var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
				return result.User.Uid;
			}
			catch (FirebaseAuthException ex)
			{
				// Giriş başarısız
				Console.WriteLine($"Giriş hatası: {ex.Message}");
				return null;
			}
		}

		public async Task<string> SignUpWithEmailAndPasswordAsync(string email, string password)
		{
			try
			{
				var auth = FirebaseAuth.DefaultInstance;
				var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
				return result.User.Uid;
			}
			catch (FirebaseAuthException ex)
			{
				// Kayıt oluşturma hatası
				Console.WriteLine($"Kayıt hatası: {ex.Message}");
				return null;
			}
		}
	}
}
