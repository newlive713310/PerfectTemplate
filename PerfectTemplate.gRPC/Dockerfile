FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV https_proxy=''
ENV HTTPS_PROXY=''
ENV no_proxy=''
ENV NO_PROXY=''

WORKDIR /src
COPY . .
RUN dotnet restore "PerfectTemplate.gRPC/PerfectTemplate.gRPC.csproj"

COPY . .
WORKDIR "/src/PerfectTemplate.gRPC"
RUN dotnet build "PerfectTemplate.gRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PerfectTemplate.gRPC.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://*:11630
EXPOSE 11630

ENTRYPOINT ["dotnet", "PerfectTemplate.gRPC.dll"]