using MagicCauldron.Api.Extension;
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


builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting();



var app = builder.Build();


#region Ocelot
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
    
app.UseOcelot().Wait(); 
#endregion

app.UseRouting();
app.RegisterAllEndpoints();
app.UseAuthorization();


app.Run();