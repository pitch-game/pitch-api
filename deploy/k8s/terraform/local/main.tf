module "namespaces" {
  source = "../modules/namespaces"
}

module "ingress" {
  source = "../modules/ingress"
}

module "secrets" {
  source = "../modules/secrets"

  identity_client_id = ""
  identity_client_secret = ""

  connection_string_service_bus = "host=rabbitmq;username=test;password=test"
  connection_string_rabbitmq_health_check = "amqp://test:test@rabbitmq:5672"
  connection_string_mongodb = "mongodb://root:example@mongodb:27017/"
}

module "mongodb" {
  source = "../modules/mongodb"

  root_password = "example"
}

module "rabbitmq" {
  source = "../modules/rabbitmq"

  auth_username = "test"
  auth_password = "test"
}