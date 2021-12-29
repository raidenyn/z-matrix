FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ZCalc.Blazor/ZCalc.Blazor.csproj", "ZCalc.Blazor/"]
RUN dotnet restore "ZCalc.Blazor/ZCalc.Blazor.csproj"
COPY . .
WORKDIR "/src/ZCalc.Blazor"
RUN dotnet build "ZCalc.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZCalc.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZCalc.Blazor.dll"]
