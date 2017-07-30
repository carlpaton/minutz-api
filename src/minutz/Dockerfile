FROM dockerdurban/aspnetcore:nodejs-aspnet-1-1-2
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "minutz.dll"]
COPY . /app
