FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LearnNetCore/LearnNetCore.csproj", "LearnNetCore/"]
RUN dotnet restore "LearnNetCore/LearnNetCore.csproj"
COPY . .
WORKDIR "/src/LearnNetCore"
RUN dotnet build "LearnNetCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnNetCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearnNetCore.dll"]
