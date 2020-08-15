resource "helm_release" "rabbitmq" {
  name       = "rabbitmq"
  repository = "https://charts.bitnami.com/bitnami"
  chart      = "rabbitmq"
  namespace  = "pitch-api"

  set {
    name  = "auth.username"
    value = "test" //TODO var
  }

  set {
    name  = "auth.password"
    value = "test" //TODO var
  }

  set {
    name  = "volumePermissions.enabled"
    value = "true"
  }
}
