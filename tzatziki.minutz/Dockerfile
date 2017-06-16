FROM microsoft/aspnetcore:1.1.2
ARG source
WORKDIR /app
EXPOSE 80
EXPOSE 1433
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "tzatziki.minutz.dll"]