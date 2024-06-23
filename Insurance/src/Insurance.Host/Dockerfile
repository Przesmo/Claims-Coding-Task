#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Insurance/src/Insurance.Host/Insurance.Host.csproj", "Insurance/src/Insurance.Host/"]
COPY ["Insurance/src/Insurance.Application/Insurance.Application.csproj", "Insurance/src/Insurance.Application/"]
COPY ["Insurance/src/Insurance.Infrastructure/Insurance.Infrastructure.csproj", "Insurance/src/Insurance.Infrastructure/"]
COPY ["Auditing/src/Auditing.Contract/Auditing.Contract.csproj", "Auditing/src/Auditing.Contract/"]
RUN dotnet restore "./Insurance/src/Insurance.Host/./Insurance.Host.csproj"
COPY . .
WORKDIR "/src/Insurance/src/Insurance.Host"
RUN dotnet build "./Insurance.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Insurance.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Insurance.Host.dll"]