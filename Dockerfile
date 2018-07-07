FROM microsoft/aspnetcore
WORKDIR /app
COPY ./release .
EXPOSE 80
ENTRYPOINT ["dotnet", "app.dll"]
