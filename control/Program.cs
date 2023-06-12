using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using control.Data;
using control.Helper;
using control.Manager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.AddDbContext<controlContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("controlContext") ?? throw new InvalidOperationException("Connection string 'controlContext' not found.")));

builder.Services.AddDistributedMemoryCache();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute("/Error/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapRazorPages();



#if !DEBUG
    Task _emailTaskResult = EMailManager.SendQueuedMailsAsync();

#endif


app.Run();


#if !DEBUG
    await _emailTaskResult;
#endif


