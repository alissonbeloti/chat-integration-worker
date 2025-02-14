FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos de projeto e restaurar dependências
COPY src/Core/Domain/Domain.csproj src/Core/Domain/
COPY src/Core/Application/Application.csproj src/Core/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Worker/Worker.csproj src/Worker/
RUN dotnet restore src/Worker/Worker.csproj

# Copiar todo o código fonte e compilar
COPY . .
RUN dotnet publish src/Worker/Worker.csproj -c Release -o out

# Build da imagem final
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Worker.dll"] 