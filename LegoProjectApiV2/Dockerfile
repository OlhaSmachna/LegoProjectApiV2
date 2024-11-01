FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LegoProjectApiV2/LegoProjectApiV2.csproj", "LegoProjectApiV2/"]
RUN dotnet restore "LegoProjectApiV2/LegoProjectApiV2.csproj"
COPY . .
WORKDIR "/src/LegoProjectApiV2"
RUN dotnet build "LegoProjectApiV2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegoProjectApiV2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegoProjectApiV2.dll", "--environment=Development"]