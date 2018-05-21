FROM microsoft/aspnetcore-build:2 AS build-env
ARG UPDATE_DLL
WORKDIR /app
# COPY src/minutz.sln .
# RUN dotnet restore

COPY . .
RUN dotnet publish src/minutz.sln  -c pipelines -o out 
# --version-suffix UPDATE_DLL
FROM microsoft/aspnetcore:2
ARG source
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/src/Api/out/ .
ENTRYPOINT ["dotnet", "Api.dll"]