//extension microsoftGraphV1_0
//provider microsoftGraphV1_0

@description('The location used for all deployed resources')
param location string = resourceGroup().location
@description('Id of the user or app to assign application roles')
param principalId string = ''
param environmentName string = ''
@secure()
param eventbus_password string
@secure()
param redis_password string


@description('Tags that will be applied to all resources')
param tags object = {}

var resourceToken = uniqueString(resourceGroup().id)

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-${resourceToken}'
  location: location
  tags: tags
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: replace('acr-${resourceToken}', '-', '')
  location: location
  sku: {
    name: 'Basic'
  }
  tags: tags
}

resource caeMiRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(containerRegistry.id, managedIdentity.id, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d'))
  scope: containerRegistry
  properties: {
    principalId: managedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
    roleDefinitionId:  subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'law-${resourceToken}'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
  tags: tags
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2024-02-02-preview' = {
  name: 'cae-${resourceToken}'
  location: location
  properties: {
    workloadProfiles: [{
      workloadProfileType: 'Consumption'
      name: 'consumption'
    }]
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
  tags: tags

  resource aspireDashboard 'dotNetComponents' = {
    name: 'aspire-dashboard'
    properties: {
      componentType: 'AspireDashboard'
    }
  }
}

module resource 'dapr.bicep' = {
  name: 'dapr'
  params: {    
    managedEnvironmentName: containerAppEnvironment.name
    eventbus_password: eventbus_password
    redis_password: redis_password
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = { 
  name: 'ain-${resourceToken}'
  location: location 
  kind: 'web' 
  properties: { 
    Application_Type: 'web'
  } 
}

var storageAccountName = 'stg${resourceToken}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = { 
  name: storageAccountName 
  location: location 
  sku: { 
    name: 'Standard_LRS' 
  } 
  kind: 'StorageV2' 
  properties: { 
    accessTier: 'Hot' 
  } 
}

var accountKey = storageAccount.listKeys().keys[0].value

resource blobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-09-01' = { 
  name: '${storageAccountName}/default/invoices'
  properties: { 
    publicAccess: 'None'
  }
  dependsOn: [ storageAccount ]
}

resource explicitContributorUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(containerAppEnvironment.id, principalId, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c'))
  scope: containerAppEnvironment
  properties: {
    principalId: principalId
    roleDefinitionId:  subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')
  }
}

// resource appDeveloperRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(subscription().id, principalId, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3'))
//   //scope: subscription()
//   properties: {
//     principalId: principalId
//     roleDefinitionId:  subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3')
//   }
// }

// resource appDeveloperRoleAssignment 'Microsoft.Graph/appRoleAssignedTo@v1.0' = {
//   appRoleId: '9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3'
//   principalId: principalId
// }

// resource appRegistration 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
//   name: 'appreg-${resourceToken}'
//   location: location
//   tags: tags
//   kind: 'AzureCLI'
//   // identity: {
//   //   type: 'UserAssigned'
//   //   userAssignedIdentities: { '${principalId}': {} }
//   // }
//   properties: {
//     azCliVersion: '2.30.0'
//     retentionInterval: 'P1D'
//     arguments: environmentName
//     scriptContent: '''
//       environmentName=$1
//       az login --identity

//       # Create the app registration
//       appId=$(az ad app create --display-name "eShop Identity Server DEV" --query appId -o tsv)
//       #--reply-urls "https://myapp.example.com/signin-oidc" --available-to-other-tenants true
      
//       # Create a service principal for the app
//       #spPassword=$(az ad sp create-for-rbac --name $appId --query password -o tsv)
      
//       # Write the outputs to the $AZ_SCRIPTS_OUTPUT_PATH file
//       #echo "appId=$appId" >> $AZ_SCRIPTS_OUTPUT_PATH
//       #echo "spPassword=$spPassword" >> $AZ_SCRIPTS_OUTPUT_PATH
//     '''
//     timeout: 'PT30M' 
//     cleanupPreference: 'OnSuccess'
//   }
// }

// resource appRegistration 'Microsoft.Graph/applications@v1.0' = {
//   displayName: 'eShop Identity Server ${environmentName}'
//   uniqueName: 'eShop Identity Server ${environmentName}'
// }

output MANAGED_IDENTITY_CLIENT_ID string = managedIdentity.properties.clientId
output MANAGED_IDENTITY_NAME string = managedIdentity.name
output MANAGED_IDENTITY_PRINCIPAL_ID string = managedIdentity.properties.principalId
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = logAnalyticsWorkspace.name
output AZURE_LOG_ANALYTICS_WORKSPACE_ID string = logAnalyticsWorkspace.id
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = containerRegistry.properties.loginServer
output AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = managedIdentity.id
output AZURE_CONTAINER_REGISTRY_NAME string = containerRegistry.name
output AZURE_CONTAINER_APPS_ENVIRONMENT_NAME string = containerAppEnvironment.name
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerAppEnvironment.id
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = containerAppEnvironment.properties.defaultDomain
output APPINSIGHTS_CONNECTIONSTRING string = applicationInsights.properties.ConnectionString
output STORAGE_CONNECTIONSTRING string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${accountKey};EndpointSuffix=core.windows.net'
//output IDENTITY_CLIENTID string = 'Clientid' //appRegistration.properties.outputs.appId
//output IDENTITY_CLIENTSECRET string = 'ClientSecret' //appRegistration.properties.outputs.spPassword
