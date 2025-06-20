FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

COPY . .
RUN dotnet restore

RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ServicioUsuarios.dll"]