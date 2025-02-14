#!/bin/bash

# Criar namespace
kubectl apply -f namespace.yaml

# Aplicar configurações
kubectl apply -f configmap.yaml
kubectl apply -f secrets.yaml
kubectl apply -f deployment.yaml
kubectl apply -f hpa.yaml

# Verificar status
kubectl get all -n chat-integration 