# Deploying to AKS

## rabbitmq
```
helm install --name rabbitmq --set rabbitmq.username=test,rabbitmq.password={password} stable/rabbitmq`
```
## mongodb
```
helm install --name mongodb --set mongodbRootPassword=example,mongodbUsername=root,mongodbPassword=example,mongodbDatabase=squad stable/mongodb
```
## ingress
```
helm install stable/nginx-ingress --name nginx-ingress --set rbac.create=true
```
```
./k8s/helm/ingress.yaml
```

## Identity secret

```
apiVersion: v1
kind: Secret
metadata:
  name: identity
type: "Opaque"
data:
  ClientId: "base64-encoded-string"
  ClientSecret: "base64-encoded-string"
```
## https
https://docs.microsoft.com/en-us/azure/aks/ingress-tls

```
./k8s/helm/certificate.yaml
```