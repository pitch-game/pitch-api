resource "kubernetes_secret" "identity" {
  metadata {
    name = "identity"
    namespace = "pitch-api"
  }

  data = {
    ClientId     = var.identity_client_id
    ClientSecret = var.identity_client_secret
  }
}

resource "kubernetes_secret" "connection-strings" {
  metadata {
    name = "connection-strings"
    namespace = "pitch-api"
  }

  data = {
    ServiceBus          = var.connection_string_service_bus
    RabbitMQHealthCheck = var.connection_string_rabbitmq_health_check
    MongoDb             = var.connection_string_mongodb
  }
}
