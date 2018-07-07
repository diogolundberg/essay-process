FROM microsoft/aspnetcore
WORKDIR /app
COPY ./release .
ENTRYPOINT ["dotnet", "app.dll"]
