FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["V1/V1.csproj", "V1/"]
RUN dotnet restore "V1/V1.csproj"
COPY . .
WORKDIR "/src/V1"
RUN dotnet build "V1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "V1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "V1.dll"]
