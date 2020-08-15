provider "kubernetes" {
}

resource "kubernetes_namespace" "pitch-api-namespace" {
  metadata {
    name = "pitch-api"
  }
}
