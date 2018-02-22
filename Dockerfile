FROM microsoft/aspnetcore-build:2 AS build-env
WORKDIR /app
COPY src/minutz.sln ./
RUN dotnet restore

COPY . ./
RUN dotnet publish src/minutz.sln -c pipelines -o out

FROM microsoft/aspnetcore:2
ARG source
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/src/Api/out/ .
ENTRYPOINT ["dotnet", "Api.dll"]