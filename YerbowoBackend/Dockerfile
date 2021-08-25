# .NET Core SDK
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101 AS dotnetcore-sdk

WORKDIR /src

# Copy Projects
COPY src/Yerbowo.Api/Yerbowo.Api.csproj ./Yerbowo.Api/
COPY src/Yerbowo.Application/Yerbowo.Application.csproj ./Yerbowo.Application/
COPY src/Yerbowo.Core/Yerbowo.Core.csproj ./Yerbowo.Core/
COPY src/Yerbowo.Domain/Yerbowo.Domain.csproj ./Yerbowo.Domain/
COPY src/Yerbowo.Fakers/Yerbowo.Fakers.csproj ./Yerbowo.Fakers/
COPY src/Yerbowo.Infrastructure/Yerbowo.Infrastructure.csproj ./Yerbowo.Infrastructure/

# .NET Core Restore
RUN dotnet restore ./Yerbowo.Api/Yerbowo.Api.csproj

# Copy All Files
COPY src .

# .NET Core Build and Publish
FROM dotnetcore-sdk as dotnetcore-build
RUN dotnet publish ./Yerbowo.Api/Yerbowo.Api.csproj -c Release -o /publish

# Angular
FROM node:12.14.0 AS angular-build
ARG ANGULAR_ENVIRONMENT
WORKDIR /wwwroot
ENV PATH /wwwroot/node_modules/.bin:$PATH
COPY src/Yerbowo.Api/wwwroot/package.json .
RUN npm install
COPY src/Yerbowo.Api/wwwroot .
RUN npm run $ANGULAR_ENVIRONMENT

# ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.1-alpine AS aspnetcore-runtime
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
COPY --from=dotnetcore-build /publish .
COPY --from=angular-build /wwwroot ./wwwroot
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Yerbowo.Api.dll"]