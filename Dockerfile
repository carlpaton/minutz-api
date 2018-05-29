FROM microsoft/aspnetcore-build:2 AS build-env
ARG UPDATE_DLL
WORKDIR /tmp
COPY src/AspnetAuthenticationRespository/AspnetAuthenticationRespository.csproj .
RUN dotnet restore
RUN rm AspnetAuthenticationRespository.csproj
WORKDIR /app
COPY . .
RUN dotnet publish src/minutz.sln  -c release -o out 
# --version-suffix UPDATE_DLL
FROM microsoft/aspnetcore:2
ARG source
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/src/Api/out/ .
ENTRYPOINT ["dotnet", "Api.dll"]