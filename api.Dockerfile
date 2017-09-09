FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-dist} .
ENTRYPOINT ["dotnet", "Api.dll"]