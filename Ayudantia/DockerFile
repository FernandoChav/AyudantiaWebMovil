# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./Ayudantia/*.csproj ./Ayudantia/
RUN dotnet restore ./Ayudantia/Ayudantia.csproj
COPY . .
RUN dotnet publish ./Ayudantia/Ayudantia.csproj -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Ayudantia.dll"]