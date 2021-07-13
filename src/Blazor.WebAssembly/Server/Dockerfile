#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8085

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Blazor.WebAssembly/Server/Blazor.WebAssembly.Server.csproj", "src/Blazor.WebAssembly/Server/"]
COPY ["src/Blazor.WebAssembly/Client/Blazor.WebAssembly.Client.csproj", "src/Blazor.WebAssembly/Client/"]
COPY ["src/Blazor.WebAssembly/Shared/Blazor.WebAssembly.Shared.csproj", "src/Blazor.WebAssembly/Shared/"]
RUN dotnet restore "src/Blazor.WebAssembly/Server/Blazor.WebAssembly.Server.csproj"
COPY . .
WORKDIR "/src/src/Blazor.WebAssembly/Server"
RUN dotnet build "Blazor.WebAssembly.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.WebAssembly.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8085
ENTRYPOINT ["dotnet", "Blazor.WebAssembly.Server.dll","--server.urls","http://*:8085"]