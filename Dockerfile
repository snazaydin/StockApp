# 1. Aşama: Kodunuzu derleyin
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["StockApp.Api/StockApp.Api.csproj", "StockApp.Api/"]
COPY ["StockApp.Data/StockApp.Data.csproj", "StockApp.Data/"]
RUN dotnet restore "StockApp.Api/StockApp.Api.csproj"
COPY . .
WORKDIR "/src/StockApp.Api"
RUN dotnet build "StockApp.Api.csproj" -c Release -o /app/build

# 2. Aşama: Uygulamanızı yayınlayın
FROM build AS publish
RUN dotnet publish "StockApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Aşama: Son imajı oluşturun
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# CRITICAL FIX: pg_isready aracını son imajda kur
# dotnet/aspnet imajına postgresql-client aracını kur
RUN apt-get update && \
    apt-get install -y postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# entrypoint.sh dosyasını konteynere kopyala ve çalıştırılabilir yap
COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh 

COPY --from=publish /app/publish .

# Başlangıç komutunu entrypoint.sh olarak ayarla
ENTRYPOINT ["./entrypoint.sh"]