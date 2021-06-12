provider "kubernetes" {
  config_path = "~/.kube/config"
}

resource "kubernetes_namespace" "pitch-api-namespace" {
  metadata {
    name = "pitch-api"
  }
}
