#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PerfectTemplate.WebApi/PerfectTemplate.WebApi.csproj", "PerfectTemplate.WebApi/"]
RUN dotnet restore "PerfectTemplate.WebApi/PerfectTemplate.WebApi.csproj"
COPY . .
WORKDIR "/src/PerfectTemplate.WebApi"
RUN dotnet build "PerfectTemplate.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PerfectTemplate.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PerfectTemplate.WebApi.dll"]