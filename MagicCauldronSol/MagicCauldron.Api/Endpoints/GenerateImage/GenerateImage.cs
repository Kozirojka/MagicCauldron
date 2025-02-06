using System.ClientModel;
using MagicCauldron.Api.Configurations;
using MagicCauldron.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenAI.Images;

namespace MagicCauldron.Api.Endpoints.GenerateImage;

public record GenerateImageRequest(List<string> Ingredients) : IParsable<GenerateImageRequest>
{
    public static GenerateImageRequest Parse(string s, IFormatProvider? provider)
    {
        var ingredients = s.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .ToList();
        return new GenerateImageRequest(ingredients);
    }

    public static bool TryParse(string s, IFormatProvider? provider, out GenerateImageRequest result)
    {
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch
        {
            result = null!;
            return false;
        }
    }
}

public class GenerateImage : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/image", async ([FromQuery] GenerateImageRequest request, IOptions<ChatgptConfiguration> options) =>
        {
            ImageClient client = new ImageClient("dall-e-3", options.Value.Key);
            
            // Обробка списку інгредієнтів
            string ingredients = string.Join(", ", request.Ingredients);
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

            GeneratedImage image = await client.GenerateImageAsync(prompt, optionOfImage);
            var urlOfImage = image.ImageUri.ToString();

            return Results.Ok(new { url = urlOfImage });
        });
    }
}