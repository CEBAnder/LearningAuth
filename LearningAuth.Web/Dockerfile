FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LearningAuth.Web/LearningAuth.Web.csproj", "LearningAuth.Web/"]
RUN dotnet restore "LearningAuth.Web/LearningAuth.Web.csproj"
COPY . .
WORKDIR "/src/LearningAuth.Web"
RUN dotnet build "LearningAuth.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningAuth.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningAuth.Web.dll"]
