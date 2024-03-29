FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["LearnNet/LearnNet.csproj", "LearnNet/"]
RUN dotnet restore "LearnNet/LearnNet.csproj"
COPY . .
WORKDIR "/src/LearnNet"
RUN dotnet build "LearnNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnNet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearnNet.dll"]
