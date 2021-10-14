FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ServerSideProgramming/ServerSideProgramming.fsproj", "ServerSideProgramming/"]
RUN dotnet restore "ServerSideProgramming/ServerSideProgramming.fsproj"
COPY . .
WORKDIR "/src/ServerSideProgramming"
RUN dotnet build "ServerSideProgramming.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerSideProgramming.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerSideProgramming.dll"]
