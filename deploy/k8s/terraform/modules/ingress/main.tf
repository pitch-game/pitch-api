provider "kubernetes" {
}

resource "helm_release" "nginx-ingress" {
  name       = "nginx-ingress"
  repository = "https://charts.bitnami.com/bitnami"
  chart      = "nginx-ingress-controller"
  namespace  = "pitch-api"

  set {
    name  = "rbac.create"
    value = true
  }
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
