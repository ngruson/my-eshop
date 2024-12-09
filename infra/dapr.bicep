param managedEnvironmentName string
@secure()
param eventbus_password string
@secure()
param redis_password string

resource managedEnvironment 'Microsoft.App/managedEnvironments@2024-10-02-preview' existing = {
  name: managedEnvironmentName 
}

resource pubsub 'Microsoft.App/managedEnvironments/daprComponents@2024-10-02-preview' = {
  name: 'pubsub'
  parent: managedEnvironment
  properties: {
    componentType: 'pubsub.rabbitmq'
    metadata: [
      {
        name: 'hostname'
        value: 'eventbus'
      }
      {
        name: 'username'
        value: 'guest'
      }
      {
        name: 'password'
        secretRef: 'eventbus-password-secret'
      }
      {
        name: 'exchangeKind'
        value: 'direct'
      }
      {
        name: 'deliveryMode'
        value: '2'
      }
      {
        name: 'durable'
        value: 'false'
      }
      {
        name: 'deleteWhenUnused'
        value: 'false'
      }
    ]
    secrets: [
      {
        name: 'eventbus-password-secret'
        value: eventbus_password
      }
    ]    
    version: 'v1'
  }
}

resource stateStore 'Microsoft.App/managedEnvironments/daprComponents@2024-10-02-preview' = {
  name: 'statestore'
  parent: managedEnvironment
  properties: {
    componentType: 'state.redis'
    metadata: [
      {
        name: 'redisHost'
        value: 'redis:6379'
      }
      {
        name: 'redisPassword'
        secretRef: 'redis-password-secret'
      }
      {
        name: 'actorStateStore'
        value: 'true'
      }
    ]
    secrets: [
      {
        name: 'redis-password-secret'
        value: redis_password
      }
    ]
    scopes: [
      'basket-api'
    ]
    version: 'v1'
  }
}
