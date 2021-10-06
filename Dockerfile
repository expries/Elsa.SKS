FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY *.sln ./
COPY /src/Elsa.SKS.Package.Services/*.csproj ./src/Elsa.SKS.Package.Services/
COPY /src/Elsa.SKS.Package.Services.DTOs/*.csproj ./src/Elsa.SKS.Package.Services.DTOs/
COPY /src/Elsa.SKS.Package.Services.Interfaces/*.csproj ./src/Elsa.SKS.Package.Services.Interfaces/
COPY /src/Elsa.SKS.Package.Services.Tests/*.csproj ./src/Elsa.SKS.Package.Services.Tests/
COPY /src/Elsa.SKS.Package.BusinessLogic/*.csproj ./src/Elsa.SKS.Package.BusinessLogic/
COPY /src/Elsa.SKS.Package.BusinessLogic.Entities/*.csproj ./src/Elsa.SKS.Package.BusinessLogic.Entities/
COPY /src/Elsa.SKS.Package.BusinessLogic.Interfaces/*.csproj ./src/Elsa.SKS.Package.BusinessLogic.Interfaces/
COPY /src/Elsa.SKS.Package.BusinessLogic.Tests/*.csproj ./src/Elsa.SKS.Package.BusinessLogic.Tests/

RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Elsa.SKS.Package.Services.dll"]
