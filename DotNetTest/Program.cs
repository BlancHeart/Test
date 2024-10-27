using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (FileStream fileStream = new("TestData.txt", FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] bytes = Convert.FromBase64String("Vibing+Day++oGfQdDtQ6v==");
                        Console.WriteLine(Convert.ToBase64String(aes.Key));
                        byte[] key ={ 0x70, 0x71, 0xb3, 0x5d, 0xf8, 0x96, 0x6e, 0xc4, 0x78, 0x48, 0x0e, 0xbe, 0x77, 0xb1, 0x7f, 0x8f, 0xcc, 0x7e, 0x6d, 0x68, 0x6a, 0x1f, 0xc3, 0xe0, 0x1a, 0x35, 0x15, 0xb5, 0x96, 0xf3, 0x26, 0x93 };
                        aes.Key = key;
                        aes.IV = bytes;
                        Console.WriteLine(Convert.ToBase64String(aes.Key));

                        byte[] iv = aes.IV;
                        Console.WriteLine(Convert.ToBase64String(aes.IV));

                        fileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new(
                            fileStream,
                            aes.CreateEncryptor(),
                            CryptoStreamMode.Write))
                        {
                            // By default, the StreamWriter uses UTF-8 encoding.
                            // To change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                encryptWriter.WriteLine("Hello World!");
                            }
                        }
                    }
                }

                Console.WriteLine("The file was encrypted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
            }

            try
            {
                using (FileStream fileStream = new("TestData.txt", FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                            if (n == 0) break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        byte[] key =
                        {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            };

                        using (CryptoStream cryptoStream = new(
                           fileStream,
                           aes.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read))
                        {
                            // By default, the StreamReader uses UTF-8 encoding.
                            // To change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                            using (StreamReader decryptReader = new(cryptoStream))
                            {
                                string decryptedMessage = await decryptReader.ReadToEndAsync();
                                Console.WriteLine($"The decrypted original message: {decryptedMessage}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
            }
        }

        public class DTO
        {
            public BitArray Key1 { get; set; }
            public BitArray Key2 { get; set; }

        }
    }
}
