# =======================
# STAGE 1: Angular build
# =======================
FROM node:20-alpine AS frontend-build

WORKDIR /app/frontend

# Copy package files separately to optimize Docker layer caching during npm install
COPY YerbowoFrontend/package.json YerbowoFrontend/package-lock.json ./
RUN npm install

# Copy all frontend source files and build the Angular app in production mode
COPY YerbowoFrontend/ ./
RUN npm run build -- --configuration production


# ===================================
# STAGE 2: .NET restore/build/publish
# ===================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build

WORKDIR /app/backend

# Copy project files separately to better leverage Docker cache for dotnet restore
COPY YerbowoBackend/src/Yerbowo.Api/Yerbowo.Api.csproj Yerbowo.Api/
COPY YerbowoBackend/src/Yerbowo.Application/Yerbowo.Application.csproj Yerbowo.Application/
COPY YerbowoBackend/src/Yerbowo.Domain/Yerbowo.Domain.csproj Yerbowo.Domain/
COPY YerbowoBackend/src/Yerbowo.Infrastructure/Yerbowo.Infrastructure.csproj Yerbowo.Infrastructure/

# Restore NuGet packages (cached unless project files change)
RUN dotnet restore Yerbowo.Api/Yerbowo.Api.csproj

# Copy the rest of the backend source code
COPY YerbowoBackend/src/ ./

# Publish the backend application in Release configuration to /app/publish
RUN dotnet publish Yerbowo.Api/Yerbowo.Api.csproj -c Release -o /app/publish


# =======================
# STAGE 3: Runtime image
# =======================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

WORKDIR /app

# Install ICU libraries needed for globalization/localization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy published backend files from the build stage
COPY --from=backend-build /app/publish ./

# Copy Angular build output into wwwroot to serve frontend static files
COPY --from=frontend-build /app/frontend/dist/yerbowo ./wwwroot/

# Expose port 80 for the application
EXPOSE 80

# Start the backend application
ENTRYPOINT ["dotnet", "Yerbowo.Api.dll"]