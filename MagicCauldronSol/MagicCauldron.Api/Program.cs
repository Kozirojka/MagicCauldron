using MagicCauldron.Api.Configurations;
using MagicCauldron.Api.Endpoints.GenerateImage;
using MagicCauldron.Api.Extension;
using MagicCauldron.Api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ChatgptConfiguration>().Bind(builder.Configuration.GetSection("Chatgpt"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

app.UseRouting();

app.RegisterAllEndpoints();


app.Run();
