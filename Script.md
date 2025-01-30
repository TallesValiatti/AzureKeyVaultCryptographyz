# 🚀 Azure Key Vault Setup and Cleanup Script

This script performs the following actions:

1. Logs into Azure.
2. Lists all available subscriptions.
3. Selects a specific subscription.
4. Creates a resource group.
5. Creates an Azure Key Vault.
6. Assigns the **Key Vault Crypto Officer** role to the signed-in user.
7. Lists role assignments for verification.
8. Deletes the resource group and all associated resources.

## 🔹 Prerequisites

- Ensure you have the **Azure CLI** installed.
- You must have sufficient permissions to create and manage resources.

---

## 📜 Script

```bash
# 🔹 Log in to Azure
az login

# 🔹 List all available subscriptions in table format
az account list --output table

# 🏷️ Set the subscription ID (replace with the actual subscription ID)
SUBSCRIPTION_ID=<SELECTED-SUBSCRIPTION>

# 🏗️ Define the resource group and Key Vault names
RESOURCE_GROUP_NAME=rg-cryptography-eastus
KEY_VAULT_NAME=kv-cryptography-eastus

# 🆔 Retrieve the object ID of the signed-in Azure AD user
USER_OBJECT_ID=$(az ad signed-in-user show --query id --output tsv)

# 🔹 Set the active Azure subscription
az account set --subscription $SUBSCRIPTION_ID

# 🏗️ Create a new resource group in the 'East US' region
az group create --name $RESOURCE_GROUP_NAME --location 'eastus'

# 🔐 Create an Azure Key Vault in the specified resource group
az keyvault create --name $KEY_VAULT_NAME --resource-group $RESOURCE_GROUP_NAME --location 'eastus' --sku standard

# 🛡️ Assign the 'Key Vault Crypto Officer' role to the signed-in user
az role assignment create --assignee $USER_OBJECT_ID --role 'Key Vault Crypto Officer' --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP_NAME/providers/Microsoft.KeyVault/vaults/$KEY_VAULT_NAME"

# 📋 List role assignments to verify the user's permissions
az role assignment list --assignee $USER_OBJECT_ID --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP_NAME/providers/Microsoft.KeyVault/vaults/$KEY_VAULT_NAME" --output table

# ❌ Delete the resource group (and all associated resources)
az group delete --name $RESOURCE_GROUP_NAME --yes
