FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV TZ="Asia/Bangkok"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EnergyMonitoring.csproj .
RUN dotnet restore "EnergyMonitoring.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EnergyMonitoring.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "EnergyMonitoring.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
# serial port support
# RUN apt-get update && apt-get install -y \
#     libgdiplus \
#     && rm -rf /var/lib/apt/lists/*
    
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EnergyMonitoring.dll"]
