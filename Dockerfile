# Use the official .NET 8.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["aranha.csproj", "."]
RUN dotnet restore "./aranha.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./aranha.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application to the /app/publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./aranha.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image to create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Switch to root user to install dependencies
USER root

# Install Playwright dependencies
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    libnss3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libxkbcommon-x11-0 \
    libgtk-3-0 \
    libgbm1 \
    libxshmfence1 \
    libasound2 \
    && rm -rf /var/lib/apt/lists/*

# Install Node.js (required by Playwright)
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get install -y nodejs

# Install Playwright CLI and browsers
RUN dotnet tool install --global Microsoft.Playwright.CLI && \
    /root/.dotnet/tools/playwright install

# Set the correct PATH for the dotnet tools
ENV PATH="$PATH:/root/.dotnet/tools"

# Switch back to the default user
USER app

# Start the application
ENTRYPOINT ["dotnet", "aranha.dll"]
