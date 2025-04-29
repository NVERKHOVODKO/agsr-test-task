FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DataGenerator/DataGenerator.csproj", "DataGenerator/"]
COPY ["Patient.Core/Patient.Core.csproj", "Patient.Core/"]
RUN dotnet restore "DataGenerator/DataGenerator.csproj"
COPY . .
WORKDIR "/src/DataGenerator"
RUN dotnet build "DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataGenerator.dll"]
