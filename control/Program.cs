using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using control.Data;
using control.Helper;
using control.Manager;
using System;
using control.Manager.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.AddDbContext<controlContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("controlContext") ?? throw new InvalidOperationException("Connection string 'controlContext' not found.")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<IDataBaseManager, DataBaseManager>();


//If load fails, we save the default values to a new file
if (!EMailManager.LoadMailSettingsFromConfig())
    EMailManager.SaveMailSettingsToConfig();

if (!GeneralSettingsManager.LoadGeneralSettingsFromConfig())
    GeneralSettingsManager.SaveGeneralSettingsToConfig();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = GeneralSettingsManager.GetUserLoginTimeout();
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
    Task _userCleanupResult =  UserStateManager.DeleteOldStatesAsync();


    var _dataBaseManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<IDataBaseManager>();

    Task _dataBaseManagerResult = _dataBaseManager.DeleteOldLoginLinksAsync();
#endif


    app.Run();

#if !DEBUG
    await _emailTaskResult;
    await _userCleanupResult;
#endif


