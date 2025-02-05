using System.ClientModel;
using MagicCauldron.Api.Configurations;
using MagicCauldron.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenAI.Images;

namespace MagicCauldron.Api.Endpoints.GenerateImage;

public sealed record GenerateImageRequest([property: FromQuery] List<string> ListOfIngredients);

public class GenerateImage() : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/image", HandleCreateOfImage);
    }
    
    private async Task<IResult> HandleCreateOfImage(IOptions<ChatgptConfiguration> options, GenerateImageRequest request)
    {
        ImageClient client = new ImageClient("dall-e-3", options.Value.Key);
        
        string ingredients = string.Join(", ", request.ListOfIngredients);
        string prompt = $"Create a realistic and high-quality image of a delicious dish made " +
                        $"using the following ingredients: {ingredients}. The dish should be visually " +
                        $"appealing, well-plated, and look professionally prepared. Use natural " +
                        $"lighting and a gourmet presentation style.";

        ImageGenerationOptions optionOfImage = new()
        {
            Quality = GeneratedImageQuality.High,
            Size = GeneratedImageSize.W1024xH1024,
            Style = GeneratedImageStyle.Vivid,
            ResponseFormat = GeneratedImageFormat.Uri
        };
        
        ClientResult<GeneratedImage> image = await client.GenerateImageAsync(prompt, optionOfImage);

        var urlOfImage = image.Value.ImageUri.ToString();
        

        return Results.Ok(new
        {
            url = urlOfImage
        });
    }
}