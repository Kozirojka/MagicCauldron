using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


#region Ocelot
builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = "OcelotConfiguration";
});
builder.Services.AddOcelot(builder.Configuration).AddAppConfiguration();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
#endregion


builder.Services.AddOpenApi();
builder.Configuration.AddJsonFile($"ocelot.json", false ,true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region Ocelot
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";

});
app.UseWebSockets();
app.UseOcelot().Wait(); 
#endregion

app.UseHttpsRedirection();
app.Run();