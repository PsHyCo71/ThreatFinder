using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Provides functionality for calculating cryptographic hashes of files.
/// </summary>
public class FileHasher
{
    /// <summary>
    /// Computes the SHA-256 hash of the file at the specified path.
    /// </summary>
    /// <param name="path">Path to the file to hash.</param>
    /// <returns>The SHA-256 hash represented as a lowercase hexadecimal string.</returns>
    public static async Task<string> ComputeSha256Async(string path)
    {
        // Opens the file for reading and computes its SHA-256 hash asynchronously.
        using var stream = File.OpenRead(path);
        byte[] byteHash = await SHA256.HashDataAsync(stream);
        return Convert.ToHexString(byteHash).ToLowerInvariant();
    }    
}