FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ARG ENVIRONMENT

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}

ARG MYSQL_SERVER
ARG MYSQL_PORT
ARG MYSQL_DATABASE
ARG MYSQL_USER
ARG MYSQL_PASSWORD

ARG ADMIN_PASSWORD

ENV ASPNETCORE_MYSQL_SERVER=${MYSQL_SERVER}
ENV ASPNETCORE_MYSQL_PORT=${MYSQL_PORT}
ENV ASPNETCORE_MYSQL_DATABASE=${MYSQL_DATABASE}
ENV ASPNETCORE_MYSQL_USER=${MYSQL_USER}
ENV ASPNETCORE_MYSQL_PASSWORD=${MYSQL_PASSWORD}

ENV ASPNETCORE_ADMIN_PASSWORD=${ADMIN_PASSWORD}

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["home_back.csproj", "./"]
RUN dotnet restore "home_back.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "home_back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "home_back.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "home_back.dll"]