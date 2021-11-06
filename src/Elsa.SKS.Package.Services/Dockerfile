FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore "src/Elsa.SKS.Package.Services/Elsa.SKS.Package.Services.csproj"

WORKDIR "/build/src/Elsa.SKS.Package.Services"
RUN dotnet build "Elsa.SKS.Package.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elsa.SKS.Package.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elsa.SKS.Package.Services.dll"]
