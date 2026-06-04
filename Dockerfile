
# --- Stage 1: Compiler ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS compiler
WORKDIR /src

COPY src/ArchGuardian.API/ArchGuardian.API.csproj src/ArchGuardian.API/
COPY src/ArchGuardian.Application/ArchGuardian.Application.csproj src/ArchGuardian.Application/
COPY src/ArchGuardian.Domain/ArchGuardian.Domain.csproj src/ArchGuardian.Domain/
COPY src/ArchGuardian.Infrastructure/ArchGuardian.Infrastructure.csproj src/ArchGuardian.Infrastructure/

RUN dotnet restore src/ArchGuardian.API/ArchGuardian.API.csproj

COPY src/ src/

RUN dotnet publish src/ArchGuardian.API/ArchGuardian.API.csproj \
    -c Release \
    -o /app/publish

# --- Stage 2: Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=compiler /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ArchGuardian.API.dll"]
