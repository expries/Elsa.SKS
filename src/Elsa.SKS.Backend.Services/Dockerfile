FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /build
COPY . .

RUN dotnet restore ./src/Elsa.SKS.Backend.Services/Elsa.SKS.Backend.Services.csproj
RUN dotnet build --no-restore ./src/Elsa.SKS.Backend.Services/Elsa.SKS.Backend.Services.csproj

FROM build as publish
RUN dotnet publish ./src/Elsa.SKS.Backend.Services/Elsa.SKS.Backend.Services.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elsa.SKS.Backend.Services.dll"]
