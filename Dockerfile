FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ARG ENVIRONMENT

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}

ARG CONNECTION_STRING
ARG DATABASE_NAME
ARG GAMES_COLLECTION_NAME
ARG ADMIN_PASSWORD

ENV ASPNETCORE_CONNECTION_STRING=${CONNECTION_STRING}
ENV ASPNETCORE_DATABASE_NAME=${DATABASE_NAME}
ENV APSNETCORE_GAMES_COLLECTION_NAME=${GAMES_COLLECTION_NAME}

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