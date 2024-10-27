using System;
using System.Security.Cryptography;

byte[] aesKey = generateAESKey();
Console.WriteLine(Convert.ToBase64String(aesKey));

static byte[] generateAESKey()
{
    using var aes = Aes.Create();
    aes.GenerateKey();
    return aes.Key;
}