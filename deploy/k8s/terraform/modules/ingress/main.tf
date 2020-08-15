provider "kubernetes" {
}

resource "kubernetes_ingress" "pitch-gateway-ingress" {
  metadata {
    name      = "pitch-gateway-ingress"
    namespace = "pitch-api"
  }

  spec {
    backend {
      service_name = "api-gateway"
      service_port = 80
    }
  }
}
