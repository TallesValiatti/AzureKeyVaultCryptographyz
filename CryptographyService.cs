using System.Text;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;

namespace AzureKeyVaultCryptography;

public class CryptographyService(string keyVaultUrl)
{
    public async Task<string> EncryptDataAsync(string key, string data)
    {
        var client = GetClient();
        var vaultKey = await EnsureKeyExistsAsync(client, key);
        var cryptoClient = client.GetCryptographyClient(vaultKey.Name, vaultKey.Properties.Version);

        var byteData = Encoding.UTF8.GetBytes(data);
        var encryptedData = await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
        return Convert.ToBase64String(encryptedData.Ciphertext);
    }

    public async Task<string> DecryptDataAsync(string key, string data)
    {
        var client = GetClient();
        var vaultKey = await EnsureKeyExistsAsync(client, key);
        var cryptoClient = client.GetCryptographyClient(vaultKey.Name, vaultKey.Properties.Version);

        var byteData = Convert.FromBase64String(data);
        var result = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
        return Encoding.UTF8.GetString(result.Plaintext);
    }
    
    public async Task<KeyVaultKey> EnsureKeyExistsAsync(string key)
    {
        var client = GetClient();
        return await EnsureKeyExistsAsync(client, key);
    }
    
    private async Task<KeyVaultKey> EnsureKeyExistsAsync(KeyClient client, string keyName)
    {
        KeyVaultKey key;
        try
        {
            key = await client.GetKeyAsync(keyName);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            key = await client.CreateRsaKeyAsync(new CreateRsaKeyOptions(keyName));
        }

        return key;
    }
    
    private KeyClient GetClient()
    {
        return new KeyClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    }
}