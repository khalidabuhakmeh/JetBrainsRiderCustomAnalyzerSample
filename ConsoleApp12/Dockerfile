FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ConsoleApp12/ConsoleApp12.csproj", "ConsoleApp12/"]
RUN dotnet restore "ConsoleApp12/ConsoleApp12.csproj"
COPY . .
WORKDIR "/src/ConsoleApp12"
RUN dotnet build "ConsoleApp12.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp12.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp12.dll"]
