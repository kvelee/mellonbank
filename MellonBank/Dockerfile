FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MellonBank/MellonBank.csproj", "MellonBank/"]
RUN dotnet restore "MellonBank/MellonBank.csproj"

COPY . .
WORKDIR "/src/MellonBank"
RUN dotnet publish "MellonBank.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MellonBank.dll"]