FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["PanierService.csproj", "./"]
RUN dotnet restore "PanierService.csproj"
COPY . .
RUN dotnet build "PanierService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PanierService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PanierService.dll"]