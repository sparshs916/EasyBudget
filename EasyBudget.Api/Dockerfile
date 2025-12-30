FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["EasyBudget.Api.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet publish "EasyBudget.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

# Install required libraries for Npgsql/Kerberos
RUN apk add --no-cache krb5-libs icu-libs

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "EasyBudget.Api.dll"]