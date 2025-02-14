# Chat Integration Worker

Este é um projeto de integração de chat que gerencia conexões com diferentes plataformas de mensageria como WhatsApp e Instagram.

## Descrição

O Chat Integration Worker é um serviço que:

- Processa mensagens de diferentes plataformas de chat (WhatsApp, Instagram)
- Executa em containers Kubernetes com alta disponibilidade
- Possui mecanismos de resiliência como circuit breaker e retry
- Escala automaticamente baseado na demanda através do HPA (Horizontal Pod Autoscaler)

## Arquitetura

O serviço é implantado como um Deployment no Kubernetes com:

- 2 réplicas para alta disponibilidade
- Probes de liveness e readiness para health check
- Limites de recursos configurados (CPU/Memória)
- Configurações gerenciadas via ConfigMaps e Secrets
- Circuit breaker configurado para tolerar até 2 falhas antes de abrir
- Política de retry com até 3 tentativas

## Configuração

As configurações sensíveis como chaves de API e credenciais são armazenadas de forma segura em Secrets do Kubernetes:

- Chave de API do WhatsApp
- Credenciais do Instagram

Outras configurações são gerenciadas via ConfigMap.

Aplicação worker para integração de mensagens do WhatsApp e Instagram usando .NET 8.

## Requisitos

- .NET SDK 8.0
- Docker
- Kubernetes (opcional)
- Conta WhatsApp Business API
- Conta Instagram API

## Configuração Inicial

1. **Clonar o Repositório**

```bash
git clone https://github.com/seu-usuario/chat-integration-worker.git
cd chat-integration-worker
```
2. **Configurar Variáveis de Ambiente**

Crie um arquivo `.env` na raiz do projeto com as seguintes variáveis:

```
ASPNETCORE_ENVIRONMENT=Development
ResilienceSettings__CircuitBreaker__ExceptionsAllowedBeforeBreaking=3
ResilienceSettings__CircuitBreaker__DurationOfBreakInSeconds=45
ResilienceSettings__Retry__MaxRetryAttempts=5
ResilienceSettings__Retry__BaseDelayMilliseconds=2000
```

3. **Executar Localmente**

```bash
docker-compose up --build
```

4. **Executar no Kubernetes**

```bash
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secrets.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/hpa.yaml
```

## Estrutura de Pastas

```
src/
├── Core/
│ ├── Domain/
│ ├── Application/      
│ └── Infrastructure/
└── Worker/
```

## Configuração do Kubernetes

1. **Namespace**

```bash
kubectl apply -f k8s/namespace.yaml
``` 

2. **ConfigMap**

```bash
kubectl apply -f k8s/configmap.yaml
```     

3. **Secrets**

```bash
kubectl apply -f k8s/secrets.yaml
``` 

4. **Deployment**

```bash
kubectl apply -f k8s/deployment.yaml
```     

5. **HPA**

```bash
kubectl apply -f k8s/hpa.yaml
``` 

## Executar no Kubernetes

```bash
kubectl apply -f k8s/apply.sh
``` 

## Verificar Status

```bash
kubectl get all -n chat-integration
```     

## Verificar Logs

```bash
kubectl logs -l app=chat-integration-worker -n chat-integration
``` 

## Verificar HPA

```bash
kubectl get hpa -n chat-integration
```  

## Verificar Deployments

```bash
kubectl get deployments -n chat-integration
``` 

## Verificar Pods

```bash
kubectl get pods -n chat-integration
```  

## Verificar Logs do Pod

```bash
kubectl logs <pod-name> -n chat-integration
```  

## Verificar Configurações

```bash
kubectl get configmap -n chat-integration
```   

## Verificar Secrets

```bash
kubectl get secrets -n chat-integration
```         






