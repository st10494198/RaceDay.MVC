using RaceDay.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services.
builder.Services.AddControllersWithViews();

// Enable session storage.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Connect the Events service to the RaceDay API.
builder.Services.AddHttpClient<EventApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// Connect the Authentication service to the RaceDay API.
builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// Connect the Categories service to the RaceDay API.
builder.Services.AddHttpClient<CategoryApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// Connect the Enrolments service to the RaceDay API.
builder.Services.AddHttpClient<EnrolmentApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// Connect the Results service to the RaceDay API.
builder.Services.AddHttpClient<ResultApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// Connect the Organiser Events service to the RaceDay API.
builder.Services.AddHttpClient<OrganiserEventApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Enable sessions before authorization.
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();