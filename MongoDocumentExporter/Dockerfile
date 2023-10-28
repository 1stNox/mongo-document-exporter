FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MongoDocumentExporter/MongoDocumentExporter.csproj", "MongoDocumentExporter/"]
RUN dotnet restore "MongoDocumentExporter/MongoDocumentExporter.csproj"
COPY . .
WORKDIR "/src/MongoDocumentExporter"
RUN dotnet build "MongoDocumentExporter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MongoDocumentExporter.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MongoDocumentExporter.dll"]
