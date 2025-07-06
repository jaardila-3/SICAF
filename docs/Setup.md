# Creación de la Estructura de Proyectos SICAF

## 1. Crear la Solución Principal

```bash
# Crear directorio raíz del proyecto
mkdir SICAF
cd SICAF

# Crear la solución .NET
dotnet new sln -n SICAF

# Crear gitignore
dotnet new gitignore

# Crear reglas de estilo de código
dotnet new editorconfig

# Crear README
echo "# SICAF - Sistema Integral de Calificación de Fases de Vuelo" > README.md

# Crear example.env
echo "ASPNETCORE_ENVIRONMENT=Development" > example.env
```

## 2. Crear Estructura de Carpetas

```bash
# Crear carpetas principales
mkdir src
mkdir tests
mkdir docs
mkdir .githooks
```

## 3. Crear Proyectos de las 5 Capas

```bash
# Navegar a la carpeta source
cd src/
```

### 3.1 Capa de Presentación (Web MVC)

```bash
# Crear proyecto MVC sin autenticación
dotnet new mvc --framework net8.0 --name SICAF.Web

# Crear proyecto MVC con autenticación Individual
# (Descomentar si se desea autenticación de Identity)
# dotnet new mvc --auth Individual --framework net8.0 --name SICAF.Web
```

### 3.2 Capa de Lógica de Negocio

```bash
# Crear proyecto de librería de clases
dotnet new classlib --framework net8.0 --name SICAF.Business
```

### 3.3 Capa de Acceso a Datos

```bash
# Crear proyecto de acceso a datos
dotnet new classlib --framework net8.0 --name SICAF.Data
```

### 3.4 Capa Común

```bash
# Crear proyecto de utilidades compartidas
dotnet new classlib --framework net8.0 --name SICAF.Common
```

### 3.5 Capa de Servicios Externos

```bash
# Crear proyecto de servicios externos
dotnet new classlib --framework net8.0 --name SICAF.Services

# Volver al directorio raíz
cd ../
```

## 4. Crear Proyectos de Pruebas

```bash
# Navegar a la carpeta tests
cd tests/
```

### 4.1 Pruebas Unitarias

```bash
# Crear proyecto de pruebas unitarias con xUnit
dotnet new xunit --framework net8.0 --name SICAF.UnitTests
```

### 4.2 Pruebas de Integración

```bash
# Crear proyecto de pruebas de integración
dotnet new xunit --framework net8.0 --name SICAF.IntegrationTests

# Volver al directorio raíz
cd ../
```

## 5. Agregar Proyectos a la Solucion y Referencias entre Proyectos

### 5.1 Agregar proyectos a la solución

```bash
# Agregar todos los proyectos a la solución
dotnet sln add src/SICAF.Web/SICAF.Web.csproj
dotnet sln add src/SICAF.Business/SICAF.Business.csproj
dotnet sln add src/SICAF.Data/SICAF.Data.csproj
dotnet sln add src/SICAF.Common/SICAF.Common.csproj
dotnet sln add src/SICAF.Services/SICAF.Services.csproj
# Agregar proyectos de pruebas
dotnet sln add tests/SICAF.UnitTests/SICAF.UnitTests.csproj
dotnet sln add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj
```

### 5.2 Referencias para SICAF.Web

```bash
# Web depende de Business, Common y Services
dotnet add src/SICAF.Web/SICAF.Web.csproj reference src/SICAF.Business/SICAF.Business.csproj
dotnet add src/SICAF.Web/SICAF.Web.csproj reference src/SICAF.Common/SICAF.Common.csproj
dotnet add src/SICAF.Web/SICAF.Web.csproj reference src/SICAF.Services/SICAF.Services.csproj
```

### 5.3 Referencias para SICAF.Business

```bash
# Business depende de Data, Common y Services
dotnet add src/SICAF.Business/SICAF.Business.csproj reference src/SICAF.Data/SICAF.Data.csproj
dotnet add src/SICAF.Business/SICAF.Business.csproj reference src/SICAF.Common/SICAF.Common.csproj
dotnet add src/SICAF.Business/SICAF.Business.csproj reference src/SICAF.Services/SICAF.Services.csproj
```

### 5.4 Referencias para SICAF.Data

```bash
# Data depende de Common
dotnet add src/SICAF.Data/SICAF.Data.csproj reference src/SICAF.Common/SICAF.Common.csproj
```

### 5.5 Referencias para SICAF.Services

```bash
# Services depende de Common
dotnet add src/SICAF.Services/SICAF.Services.csproj reference src/SICAF.Common/SICAF.Common.csproj
```

### 5.6 Referencias para Pruebas Unitarias

```bash
# UnitTests referencia todos los proyectos
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj reference src/SICAF.Web/SICAF.Web.csproj
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj reference src/SICAF.Business/SICAF.Business.csproj
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj reference src/SICAF.Data/SICAF.Data.csproj
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj reference src/SICAF.Common/SICAF.Common.csproj
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj reference src/SICAF.Services/SICAF.Services.csproj
```

### 5.7 Referencias para Pruebas de Integración

```bash
# IntegrationTests referencia principalmente Web y Data
dotnet add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj reference src/SICAF.Web/SICAF.Web.csproj
dotnet add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj reference src/SICAF.Data/SICAF.Data.csproj
dotnet add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj reference src/SICAF.Common/SICAF.Common.csproj
```

## 6. Instalar Paquetes NuGet Principales

### 6.1 Para SICAF.Data (Entity Framework Core)

```bash
# Paquetes de Entity Framework Core para MySQL
dotnet add src/SICAF.Data/SICAF.Data.csproj package Microsoft.EntityFrameworkCore --version 8.0.17
dotnet add src/SICAF.Data/SICAF.Data.csproj package MySql.EntityFrameworkCore --version 8.0.14
```

### 6.2 Para SICAF.Common (Validación)

```bash
# FluentValidation para validaciones
dotnet add src/SICAF.Common/SICAF.Common.csproj package FluentValidation --version 12.0.0
```

### 6.3 Para SICAF.Web (Logging y Variables de Entorno)

```bash
# Serilog para logging
dotnet add src/SICAF.Web/SICAF.Web.csproj package Serilog.AspNetCore --version 8.0.3

# DotNetEnv para variables de entorno
dotnet add src/SICAF.Web/SICAF.Web.csproj package DotNetEnv --version 3.1.1

# Entity Framework para migraciones
dotnet add src/SICAF.Web/SICAF.Web.csproj package Microsoft.EntityFrameworkCore.Tools --version 8.0.17
```

### 6.4 Para SICAF.Business (Lógica de Negocio)

```bash
# MediatR para mediación de comandos y consultas
dotnet add src/SICAF.Business/SICAF.Business.csproj package MediatR --version 13.0.0
```

### 6.5 Para SICAF.Services (Servicios Externos)

```bash
# Para servicios de email y almacenamiento
dotnet add src/SICAF.Services/SICAF.Services.csproj package Microsoft.Extensions.DependencyInjection.Abstractions --version 8.0.2
```

### 6.6 Para Pruebas

```bash
# Paquetes para pruebas unitarias
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj package Moq
dotnet add tests/SICAF.UnitTests/SICAF.UnitTests.csproj package coverlet.collector

# Paquetes para pruebas de integración
dotnet add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj package Microsoft.EntityFrameworkCore.InMemory
dotnet add tests/SICAF.IntegrationTests/SICAF.IntegrationTests.csproj package coverlet.collector
```

## 7. Crear Estructura de Carpetas Internas

### 7.1 SICAF.Web

```bash
# Crear carpetas para organización del proyecto Web
cd src/SICAF.Web
mkdir Areas/Account
mkdir Areas/Academic
mkdir Areas/Admin
mkdir Middleware
mkdir Extensions
cd ../..
```

### 7.2 SICAF.Business

```bash
# Crear carpetas para servicios y validadores
cd src/SICAF.Business
mkdir Behaviors
mkdir CQRS
mkdir ExternalServices
mkdir DomainServices
mkdir Interfaces
mkdir Validators
cd ../..
```

### 7.3 SICAF.Data

```bash
# Crear carpetas para acceso a datos
cd src/SICAF.Data
mkdir Context
mkdir Entities
mkdir Repositories
mkdir Configurations
mkdir Migrations
cd ../..
```

### 7.4 SICAF.Common

```bash
# Crear carpetas para utilidades compartidas
cd src/SICAF.Common
mkdir ServiceRegistration
mkdir Extensions
mkdir Helpers
mkdir Constants
mkdir Enums
mkdir Interfaces
cd ../..
```

### 7.5 SICAF.Services

```bash
# Crear carpetas para servicios externos
cd src/SICAF.Services
mkdir Email
mkdir ServiceRegistration
mkdir Interfaces
cd ../..
```

## 8. Verificar la Estructura

```bash
# Compilar toda la solución para verificar que todo está configurado correctamente
dotnet build

# Restaurar paquetes NuGet
dotnet restore

# Verificar la estructura de la solución
dotnet sln list
```

## 9. Próximos Pasos

1. **Configurar Program.cs** en SICAF.Web con DI, autenticación y Serilog
2. **Crear DbContext** en SICAF.Data
3. **Definir entidades** del dominio
4. **Implementar repositorios** y Unit of Work
5. **Crear servicios de negocio**
6. **Configurar áreas MVC** con controladores y vistas
7. **Configurar variables de entorno** (.env)
8. **Ejecutar primera migración**