FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "Web2.csproj" "Web2.csproj"
RUN dotnet restore "Web2.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Web2.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Web2.csproj" -c Release -o /app

FROM base AS final

WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "Web2.dll", "--environment=X"]