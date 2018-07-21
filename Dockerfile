FROM microsoft/aspnetcore-build:2 AS build-env
ARG UPDATE_DLL
RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
WORKDIR /tmp
COPY src/Interface/Interface.csproj .
RUN dotnet restore
RUN rm Interface.csproj
COPY src/Models/Models.csproj .
RUN dotnet restore
RUN rm Models.csproj
COPY src/MinutzEncryption/MinutzEncryption.csproj .
RUN dotnet restore
RUN rm MinutzEncryption.csproj
COPY src/Notifications/Notifications.csproj .
RUN dotnet restore
RUN rm Notifications.csproj
COPY src/Reports/Reports.csproj .
RUN dotnet restore
RUN rm Reports.csproj
COPY src/SqlRepository/SqlRepository.csproj .
RUN dotnet restore
RUN rm SqlRepository.csproj
COPY src/AspnetAuthenticationRespository/AspnetAuthenticationRespository.csproj .
RUN dotnet restore
RUN rm AspnetAuthenticationRespository.csproj
COPY src/AuthenticationRepository/AuthenticationRepository.csproj .
RUN dotnet restore
RUN rm AuthenticationRepository.csproj
COPY src/Core/Core.csproj .
RUN dotnet restore
RUN rm Core.csproj
COPY src/Api/Api.csproj .
RUN dotnet restore
RUN rm Api.csproj
COPY src/Tests/Tests.csproj .
RUN dotnet restore
RUN rm Tests.csproj

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