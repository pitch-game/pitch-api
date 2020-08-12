provider "kubernetes" {
}

resource "kubernetes_ingress" "example_ingress" {
  metadata {
    name = "pitch-gateway-ingress"
  }

  spec {
    backend {
      service_name = "api-gateway"
      service_port = 80
    }
  }
}