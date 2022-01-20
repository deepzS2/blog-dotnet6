FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["blog.csproj", "./"]
RUN dotnet restore "blog.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog.dll"]
