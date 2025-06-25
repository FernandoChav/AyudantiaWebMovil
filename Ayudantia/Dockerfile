# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar el .csproj desde la raíz del repo
COPY Ayudantia/*.csproj ./
COPY Ayudantia/*.sln ./

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código fuente
COPY Ayudantia/. ./
RUN dotnet publish -c Release -o out

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "Ayudantia.dll"]
