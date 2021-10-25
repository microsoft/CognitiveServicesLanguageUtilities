# Validating User Inputs

In order to make sure the integration tool works flawlessly, we created this document to guide and help you make sure all your inputs are correct.

## Schema.json file
In order to comply with Cognitive Search, your Custom Text app schema must adhere to the following:
- Entity names: must only contain letters and digits (no spaces or special characters)

## Configs.json file
This section helps you create the required Azure services and also helps you obtain the required service secrets.

### Create Custom Text App

### Storage Account
#### **Create Resource**
Full article [here](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).

Go to azure portal, and create new resource
search for storage account
![create_data_store_1](./Assets/storage/create_storage_account_1.bmp)
click create
![create_data_store_2](./Assets/storage/create_storage_account_2.bmp)
fill in the resource info
![create_data_store_3](./Assets/storage/create_storage_account_3.bmp)
create new container
![create_container_1](./Assets/storage/create_container_1.bmp)

#### **Get Secrets**
Go to your storage account page, then
click on access keys
![get_storage_account_secrets_1](./Assets/storage/get_storage_account_secrets_1.bmp)

### Azure Function
#### **Create Resource**
Go to azure portal, and create new resource
search for azure function
![create_azure_function_1](./Assets/function/create_azure_function_1.bmp)
click create
![create_azure_function_2](./Assets/function/create_azure_function_2.bmp)
fill in the resource info
![create_azure_function_3](./Assets/function/create_azure_function_3.bmp)

#### **Get Secrets**
Go to your function page, then get function url
![get_function_secrets_1](./Assets/function/get_function_secrets_1.bmp)
![get_function_secrets_2](./Assets/function/get_function_secrets_2.bmp)


### Cognitive Search
#### **Create Resource**
Go to azure portal, and create new resource
search for cognitive search
![create_cognitive_search_resource_1](./Assets/cognitive_search/create_cognitive_search_resource_1.bmp)
click create
![create_cognitive_search_resource_2](./Assets/cognitive_search/create_cognitive_search_resource_2.bmp)
fill in the resource info
![create_cognitive_search_resource_3](./Assets/cognitive_search/create_cognitive_search_resource_3.bmp)

#### **Get Secrets**
Go to your cognitive search page, then
get resource url
![get_cognitive_search_secrets_1](./Assets/cognitive_search/get_cognitive_search_secrets_1.bmp)
get resource key
![get_cognitive_search_secrets_2](./Assets/cognitive_search/get_cognitive_search_secrets_2.bmp)


### Custom Text
#### **Create Resource**
See this tutorial on how to create a CT resource and train your app

#### **Get Secrets**
Go to your custom text resource page, then
get resource url and key
![get_custom_text_secrets_1](./Assets/custom_text/get_custom_text_secrets_1.bmp)
