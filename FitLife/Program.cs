using FirebaseAdmin;
using FitLife.Firebase;
using FitLife.Repositories.Abstracts;
using FitLife.Repositories.Concretes;
using FitLife.Services.Abstracts;
using FitLife.Services.Concretes;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();

var firebaseConfigPath = "C:\\Users\\alier\\source\\repos\\FitLife\\FitLife\\fitlife-b940d-firebase-adminsdk-4g1gz-9f3498c82b.json";
FirebaseApp.Create(new AppOptions
{
	Credential = GoogleCredential.FromFile(firebaseConfigPath),
});

// Firebase Authentication servisini ekleyin
builder.Services.AddScoped<FirebaseAuthService>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
