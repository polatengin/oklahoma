RESOURCE_GROUP_NAME="test-${RANDOM}-rg"
STORAGE_ACCOUNT_NAME="teststrgacct${RANDOM}"

az group create \
  --name "${RESOURCE_GROUP_NAME}" \
  --location "westus2"

az storage account create \
  --name "${STORAGE_ACCOUNT_NAME}" \
  --resource-group "${RESOURCE_GROUP_NAME}" \
  --location "westus2" \
  --sku "Standard_LRS" \
  --kind "StorageV2"

az storage table create \
  --name "customers" \
  --account-name "${STORAGE_ACCOUNT_NAME}" \
  --account-key $(az storage account keys list --resource-group "${RESOURCE_GROUP_NAME}" --account-name "${STORAGE_ACCOUNT_NAME}" --query "[0].value" --output tsv)

export AZURE_STORAGE_ACCOUNT_NAME="${STORAGE_ACCOUNT_NAME}"
export AZURE_STORAGE_ACCOUNT_KEY=$(az storage account keys list --resource-group "${RESOURCE_GROUP_NAME}" --account-name "${STORAGE_ACCOUNT_NAME}" --query "[0].value" --output tsv)
export AZURE_STORAGE_ACCOUNT_TABLE_NAME="customers"
