provider "helm" {
  kubernetes {
    config_path = "~/.kube/config"
  }
}

resource "helm_release" "rabbitmq" {
  name       = "rabbitmq"
  repository = "https://charts.bitnami.com/bitnami"
  chart      = "rabbitmq"
  namespace  = "pitch-api"

  set {
    name  = "auth.username"
    value = var.auth_username
  }

  set {
    name  = "auth.password"
    value = var.auth_password
  }

  set {
    name  = "volumePermissions.enabled"
    value = "true"
  }
}
