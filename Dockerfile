# Usa la imagen del SDK de .NET 8.0 para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copia los archivos de proyecto y solución y restaura las dependencias
# Copiar los .csproj y .sln primero aprovecha el almacenamiento en caché de capas de Docker
COPY *.sln .
COPY SisNikosPizza/*.csproj ./SisNikosPizza/
COPY SisNikosPizza.Domain/*.csproj ./SisNikosPizza.Domain/
COPY SisNikosPizza.Infraestructure/*.csproj ./SisNikosPizza.Infraestructure/
COPY SisNikosPizza.Repository/*.csproj ./SisNikosPizza.Repository/
COPY SisNikosPizza.Utilidades/*.csproj ./SisNikosPizza.Utilidades/
RUN dotnet restore "SisNikosPizza.sln"

# Copia el resto del código fuente
COPY . .
WORKDIR "/source/SisNikosPizza"
RUN dotnet publish "SisNikosPizza.csproj" -c Release -o /app/publish

# Usa la imagen de ASP.NET Core runtime para la imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render establece la variable de entorno PORT.
# ASP.NET Core la recogerá automáticamente.
EXPOSE 80
EXPOSE 443

# Define el punto de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "SisNikosPizza.dll"]
