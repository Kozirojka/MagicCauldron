using MagicCauldron.Api.Configurations;
using MagicCauldron.Api.Endpoints.GenerateImage;
using MagicCauldron.Api.Extension;
using MagicCauldron.Api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ChatgptConfiguration>().Bind(builder.Configuration.GetSection("Chatgpt"));
builder.Services.AddOpenApi();
builder.Services.AddTransient<IEndpoint, GenerateImage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.RegisterAllEndpoints();


app.Run();
