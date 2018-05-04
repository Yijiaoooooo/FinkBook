FROM microsoft/aspnetcore-build AS build-env
WORKDIR /code
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=build-env /code/out ./

EXPOSE 8001
ENTRYPOINT ["dotnet", "User.Api.dll"]
