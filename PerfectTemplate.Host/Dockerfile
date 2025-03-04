#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PerfectTemplate.Host/PerfectTemplate.Host.csproj", "PerfectTemplate.Host/"]
RUN dotnet restore "./PerfectTemplate.Host/PerfectTemplate.Host.csproj"
COPY . .
WORKDIR "/src/PerfectTemplate.Host"
RUN dotnet build "./PerfectTemplate.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PerfectTemplate.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PerfectTemplate.Host.dll"]


#
#FROM nexus-dev.eub.kz:8093/dotnet/sdk AS build
#
#ENV https_proxy='http://proxy.eub.kz:8080'
#ENV HTTPS_PROXY='http://proxy.eub.kz:8080'
#ENV no_proxy='localhost,127.0.0.1,eub.kz,.eub.kz,*.eub.kz'
#ENV NO_PROXY='localhost,127.0.0.1,eub.kz,.eub.kz,*.eub.kz'
#
#WORKDIR /src
#COPY . .
#RUN dotnet restore "Eub.Service.Generic.Scb.gRPC/Eub.Service.Generic.Scb.gRPC.csproj"
#
#COPY . .
#WORKDIR "/src/Eub.Service.Generic.Scb.gRPC"
#RUN dotnet build "Eub.Service.Generic.Scb.gRPC.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Eub.Service.Generic.Scb.gRPC.csproj" -c Release -o /app/publish
#
#FROM nexus-dev.eub.kz:8093/dotnet/aspnet:8.0
#WORKDIR /app
#COPY --from=publish /app/publish .
#COPY grpc_health_probe /bin/grpc_health_probe
#RUN chmod +x /bin/grpc_health_probe
#
#ENV ASPNETCORE_URLS http://*:11610;http://*:11611
#EXPOSE 11610
#EXPOSE 11611
#
#ENTRYPOINT ["dotnet", "Eub.Service.Generic.Scb.gRPC.dll"]




#rpc GetFamilyInfo (GetFamilyInfoRequest) returns (GetFamilyInfoResponse){
	   #option (google.api.http) = {
		  #get: "/GetFamilyInfo/{Iin}"
		#};
   #};
   #rpc GetClientDataByIin (GetClientDataByIinRequest) returns (GetClientDataByIinReply){
	   #option (google.api.http) = {
		  #get: "/GetClientDataByIin/{Iin}"
		#};
    #};
    #rpc GetClientTokenByIin (GetClientTokenByIinRequest) returns (GetClientTokenByIinReply){
	   #option (google.api.http) = {
		  #get: "/GetClientTokenByIin/{Iin}"
		#};
    #};
    #rpc GetClientDataByIinWithToken (GetClientDataByIinWithTokenRequest) returns (GetClientDataByIinReply){
	   #option (google.api.http) = {
		  #get: "/GetClientDataByIinWithToken/{Iin}/{Token}"
		#};
    #};
    #rpc GetProfile (GetProfileRequest) returns (GetOnlineAccessReply){
	   #option (google.api.http) = {
		  #get: "/GetProfile/{Iin}"
		#};
    #};
    #rpc AccessDocument (AccessDocumentRequest) returns (GetOnlineAccessReply){
	   #option (google.api.http) = {
		  #get: "/AccessDocument/{Iin}/{DocumentType}"
		#};
    #};
    #rpc GetDigitalDocuments (GetDigitalDocumentsRequest) returns (GetDigitalDocumentsReply){
	   #option (google.api.http) = {
		  #get: "/GetDigitalDocuments/{Iin}/{Code}/{RequestNumber}/{ConsentConfirmed}"
		#};
    #};
    #rpc GetDigitalDocumentsList (GetDigitalDocumentsListRequest) returns (GetDigitalDocumentsListReply){
	   #option (google.api.http) = {
		  #get: "/GetDigitalDocumentsList"
		#};
    #};
#