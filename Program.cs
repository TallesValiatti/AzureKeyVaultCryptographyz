 using AzureKeyVaultCryptography;

 var keyName = "my-key";
 var data = "123456789abcdefg";
 
 var service = new CryptographyService(Environment.GetEnvironmentVariable("AzureKeyVaultUrl")!);
 
 var key = await service.EnsureKeyExistsAsync(keyName);
 Console.WriteLine("Key name: " + key.Name);
 Console.WriteLine("Key type: " + key.KeyType);
 
 var encryptedData = await service.EncryptDataAsync(keyName, data);
 Console.WriteLine("Encrypted data: " + encryptedData);
 
 var decryptedData = await service.DecryptDataAsync(keyName, encryptedData);
 Console.WriteLine("Decrypted data: " + decryptedData);
 
 