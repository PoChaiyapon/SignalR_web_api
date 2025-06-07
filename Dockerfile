FROM mtl2app-dkr-reg.co.murata.local/dotnet-linux/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM --platform=$BUILDPLATFORM mtl2app-dkr-reg.co.murata.local/dotnet-linux/sdk:6.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
