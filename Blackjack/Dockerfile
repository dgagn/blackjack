FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Blackjack/Blackjack.csproj", "Blackjack/"]
RUN dotnet restore "Blackjack/Blackjack.csproj"
COPY . .
WORKDIR "/src/Blackjack"
RUN dotnet build "Blackjack.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blackjack.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blackjack.dll"]
