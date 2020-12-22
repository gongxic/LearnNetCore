FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LearnNet5/LearnNet5.csproj", "LearnNet5/"]
RUN dotnet restore "LearnNet5/LearnNet5.csproj"
COPY . .
WORKDIR "/src/LearnNet5"
RUN dotnet build "LearnNet5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnNet5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearnNet5.dll"]
