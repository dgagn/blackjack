FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["V2/V2.csproj", "V2/"]
RUN dotnet restore "V2/V2.csproj"
COPY . .
WORKDIR "/src/V2"
RUN dotnet build "V2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "V2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "V2.dll"]
