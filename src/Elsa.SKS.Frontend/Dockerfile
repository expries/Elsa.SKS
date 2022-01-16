FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /build
COPY . .

RUN dotnet restore ./src/Elsa.SKS.Frontend/Elsa.SKS.Frontend.csproj
RUN dotnet build --no-restore ./src/Elsa.SKS.Frontend/Elsa.SKS.Frontend.csproj

FROM build as publish
RUN dotnet publish ./src/Elsa.SKS.Frontend/Elsa.SKS.Frontend.csproj -c Release -o /app/publish

FROM nginx:alpine
COPY --from=publish /app/publish/wwwroot/. /usr/share/nginx/html
