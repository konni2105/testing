
using System.Security.Cryptography;

var secretKey = Convert.ToBase64String(
    RandomNumberGenerator.GetBytes(32));

Console.WriteLine(secretKey);
