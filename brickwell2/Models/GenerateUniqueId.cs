using System;
namespace brickwell2.Models;

public class GenerateUniqueId
{
    private static readonly Random _random = new Random();

    public static string UniqueIdGenerator()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int randomNumber = _random.Next(1000, 9999); // Generate a random 4-digit number

        return $"{timestamp}-{randomNumber}";
    }
}