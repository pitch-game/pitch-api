resource "helm_release" "mongodb" {
  name       = "mongodb"
  repository = "https://charts.bitnami.com/bitnami"
  chart      = "mongodb"

  set {
    name  = "auth.rootPassword"
    value = var.root_password
  }

  set {
    name  = "volumePermissions.enabled"
    value = "true"
  }

  set {
      name = "global.namespaceOverride"
      value = "pitch-api" //todo pitch-infra?
  }
}
