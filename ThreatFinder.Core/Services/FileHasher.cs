using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

public class FileHasher
{
    public static async Task<string> ComputeSha256Async(string path)
    {
        using var stream = File.OpenRead(path);
        byte[] byteHash = await SHA256.HashDataAsync(stream);
        return Convert.ToHexString(byteHash).ToLowerInvariant();
    }    
}