# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "AllulExpressClientAPI.csproj"
RUN dotnet publish "AllulExpressClientAPI.csproj" -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "AllulExpressClientAPI.dll"]
