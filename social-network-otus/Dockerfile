#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["social-network-otus/social-network-otus.csproj", "social-network-otus/"]
RUN dotnet restore "social-network-otus/social-network-otus.csproj"
COPY . .
WORKDIR "/src/social-network-otus"
RUN dotnet build "social-network-otus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "social-network-otus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "social-network-otus.dll"]