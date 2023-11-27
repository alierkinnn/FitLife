using FirebaseAdmin;
using FitLife.Repositories.Abstracts;
using FitLife.Repositories.Concretes;
using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IHesapRepo, HesapRepo>();
builder.Services.AddScoped<IHesapService, HesapService>();

builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = "Your_External_Scheme"; // Eksikse bu satýrý ekleyin
})
	.AddCookie(options =>
	{
		// Cookie yetkilendirme konfigürasyonlarý...
	});

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

//var firebaseConfigPath = "C:\\Users\\alier\\source\\repos\\FitLife\\FitLife\\fitlife-b940d-firebase-adminsdk-4g1gz-9f3498c82b.json";
//FirebaseApp.Create(new AppOptions
//{
//	Credential = GoogleCredential.FromFile(firebaseConfigPath),
//});

var credential = GoogleCredential.FromFile("fitlife-b940d-firebase-adminsdk-4g1gz-95401c90cb.json");
var firebaseApp = FirebaseApp.Create(new AppOptions
{
	Credential = credential,
});

builder.Services.AddSingleton(firebaseApp);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Hesap}/{action=GirisYap}/{id?}");

app.Run();
