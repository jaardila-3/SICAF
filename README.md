# SICAF - Sistema Integral de Calificación de Fases de Vuelo

## Descripción General
Este proyecto introduce un sistema integral para la gestión del programa académico de tecnología de aviación policial, que permite la administración de usuarios, seguimiento académico, gestión de cursos y evaluaciones de fases de vuelo.

El sistema incluye:
- Gestión personalizada de usuarios con tablas `Users`, `Roles`, y `UserRoles`.
- Autenticación y autorización usando autenticación basada en cookies configurada en `Program.cs`.
- Funcionalidad de gestión de cuentas en el área `Account`, incluyendo login, registro y logout.
- Módulo de Seguimiento Académico
- Módulo de Calificación de Fases de Vuelo
- Módulo de Gestión de Instructores y Estudiantes

## Arquitectura de Cinco Capas:

- **Capa Web (SICAF.Web)**: Capa de presentación con ASP.NET Core MVC, áreas, controladores, vistas y modelos.
- **Capa de Negocio (SICAF.Business)**: Capa de lógica de negocio con servicios para las reglas centrales de la aplicación.
- **Capa de Datos (SICAF.Data)**: Capa de acceso a datos con Entity Framework Core, repositorios genéricos y Unit of Work para Base de Datos MySQL.
- **Capa Común (SICAF.Common)**: Utilidades compartidas, constantes, extensiones, helpers y modelos (ej. DTOs).
- **Capa de Servicios (SICAF.Services)**: Capa para integraciones con servicios externos con interfaces e implementaciones.

### Patrones de Diseño:
- **Patrón Repository Genérico**: Abstrae el acceso a datos con `IRepository<T>` reutilizable e implementaciones concretas `Repository<T>`.
- **Patrón Unit of Work**: Gestiona transacciones y coordina múltiples repositorios a través de `IUnitOfWork`.

### Tecnologías y Dependencias:
- **Framework**: ASP.NET Core 8.0 con autenticación basada en cookies.
- **Acceso a Datos**: Entity Framework Core 8.0 con `MySql.EntityFrameworkCore` para integración con Base de Datos MySQL, usando un flujo de trabajo Code First para la creación del esquema de base de datos.
- **Base de Datos**: MySQL 8.0 o superior, requiriendo `MySql.EntityFrameworkCore`.
- **Gestión de Entorno**: `DotNetEnv` para cargar variables de entorno (ej. cadenas de conexión MySQL) desde un archivo `.env` en desarrollo.
- **Logging**: `Serilog` para logging estructurado, configurable para escribir a archivos o Base de Datos MySQL.
- **Validación**: `FluentValidation` para validaciones complejas de negocio y datos, integrado en `SICAF.Common`.
- **Localización**: Configurado para cultura Español (Colombia, "es-CO"), manejando separadores decimales (coma) y formatos de fecha.

### Mejores Prácticas:
- **Inyección de Dependencias**: Usa la DI integrada de ASP.NET Core para bajo acoplamiento entre capas.
- **Manejo de Excepciones**: Implementa un manejador global de excepciones usando UseExceptionHandler o middleware personalizado, con páginas de error (`Error.cshtml`) y mensajería de errores basada en sesión.
- **Logging**: Centraliza el logging de errores y actividades con Serilog, configurable para almacenamiento en archivos y Base de Datos MySQL.
- **Pruebas**: Soporta pruebas unitarias e integración en `SICAF.UnitTests` y `SICAF.IntegrationTests`, con repositorios y servicios que se pueden mockear.
- **Estructura de Carpetas**: Mantiene una estructura limpia y modular con carpetas dedicadas para middlewares, validadores, excepciones y modelos.
- **Seguridad**: Hash de contraseñas personalizado y autorización basada en roles.
- **Rendimiento**: Uso de async/await para operaciones asíncronas, LINQ para consultas optimizadas, y paralelismo cuando es necesario.
- **Nota**: Recuerda remover headers HTTP que puedan exponer información sensible sobre tu aplicación ASP.NET Core 8, como `Server` y `X-Powered-By`.

## 📋 Funcionalidades por Módulo

### Gestión de Usuarios y Autenticación
- **Registro y autenticación** de usuarios con roles diferenciados
- **Gestión de perfiles** de Administradores, Instructores y Estudiantes
- **Restablecimiento de contraseñas** seguro
- **Auditoría de actividades** del sistema

### Módulo Académico
- **Gestión de cursos** de aviación policial
- **Administración de fases de vuelo** (Fase Básica, Intermedia, Avanzada)
- **Seguimiento del progreso estudiantil** en tiempo real
- **Calificación de misiones** y ejercicios prácticos

### Módulo de Calificaciones
- **Sistema de evaluación** flexible por fases
- **Registro de calificaciones** de vuelo
- **Generación de reportes** académicos individuales y grupales
- **Historial académico** completo

### Roles del Sistema
- **Administrador**: Gestión completa del sistema y usuarios
- **Administrador Académico**: Gestión de cursos y estudiantes
- **Instructor**: Evaluación y seguimiento de estudiantes
- **Estudiante**: Acceso a calificaciones y progreso
- **Líder de Vuelo**: Supervisión académica especializada

## 🛠️ Tecnologías

### Stack Principal
- **.NET 8.0** - Framework base
- **ASP.NET Core MVC** - Patrón Model-View-Controller
- **Entity Framework Core 8.0** - ORM con Code First
- **MySQL 8.0+** - Base de datos relacional
- **MySQL.EntityFrameworkCore** - Provider para MySQL

### Herramientas de Desarrollo
- **FluentValidation** - Validación de modelos
- **Serilog** - Logging estructurado
- **DotNetEnv** - Gestión de variables de entorno
- **Bootstrap 5** - Framework CSS para UI responsiva

### Pruebas y Calidad
- **xUnit** - Framework de pruebas unitarias
- **Moq** - Librería de mocking
- **FluentAssertions** - Assertions fluidas para pruebas

## 📋 Prerrequisitos

Antes de clonar y ejecutar este proyecto, asegúrate de tener instalado lo siguiente:

- **.NET SDK 8.0** o posterior
- **MySQL 8.0** o superior (ej. MySQL Community Server)
- **Visual Studio 2022**, **Visual Studio Code** o **JetBrains Rider**
- **Git** para control de versiones
- **MySQL Workbench** (opcional, para administración de base de datos)

## 🚀 Comenzando

### 1. Clonar el Repositorio
```bash
git clone https://github.com/jaardila-3/SICAF.git
cd SICAF
```

### 2. Abrir el Proyecto
Abre la solución en tu IDE preferido:
- **Visual Studio**: Abrir `SICAF.sln`
- **VS Code**: Abrir la carpeta del proyecto
- **Rider**: Abrir `SICAF.sln`

### 3. Restaurar Dependencias
```bash
dotnet restore
```

### 4. Instalar Herramientas de EF Core
Si no las tienes instaladas globalmente:
```bash
dotnet tool install --global dotnet-ef
```

### 5. Configurar Variables de Entorno

Crear un archivo `.env` en la raíz del proyecto basado en `example.env`.

### 6. Configurar Base de Datos MySQL

#### Crear Base de Datos
```sql
CREATE DATABASE SICAF_DB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'sicaf_user'@'localhost' IDENTIFIED BY 'tu_password_segura';
GRANT ALL PRIVILEGES ON SICAF_DB.* TO 'sicaf_user'@'localhost';
FLUSH PRIVILEGES;
```

### 7. Ejecutar Migraciones

Desde la raíz del proyecto (no desde capas individuales):

#### Crear una nueva migración:
```bash
dotnet ef migrations add InicialMigration -s src/SICAF.Web/ -p src/SICAF.Data/
```

#### Aplicar migraciones a la base de datos:
```bash
dotnet ef database update -p src/SICAF.Data/
```

#### Remover una migración (si es necesario):
```bash
dotnet ef migrations remove --project src/SICAF.Data/
```

### 8. Compilar y Ejecutar

```bash
dotnet build
dotnet run --project src/SICAF.Web/
```

La aplicación estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

## 📁 Estructura del Proyecto

```
SICAF/
├── src/
│   ├── SICAF.Web/                    # Capa de Presentación (MVC)
│   │   ├── Areas/
│   │   │   ├── Account/              # Área de gestión de cuentas
│   │   │   ├── Academic/             # Área académica
│   │   │   └── Admin/                # Área de administración
│   │   ├── Controllers/              # Controladores principales
│   │   ├── Views/                    # Vistas Razor
│   │   ├── wwwroot/                  # Archivos estáticos
│   │   └── Program.cs                # Punto de entrada
│   │
│   ├── SICAF.Business/               # Capa de Lógica de Negocio
│   │   ├── Services/                 # Servicios de negocio
│   │   ├── Interfaces/               # Interfaces de servicios
│   │   └── Validators/               # Validadores FluentValidation
│   │
│   ├── SICAF.Data/                   # Capa de Acceso a Datos
│   │   ├── Context/                  # Contexto de EF Core
│   │   ├── Entities/                 # Entidades del dominio
│   │   ├── Repositories/             # Implementación de repositorios
│   │   ├── Configurations/           # Configuraciones de EF
│   │   └── Migrations/               # Migraciones de base de datos
│   │
│   ├── SICAF.Common/                 # Capa Común
│   │   ├── DTOs/                     # Data Transfer Objects
│   │   ├── Extensions/               # Métodos de extensión
│   │   ├── Helpers/                  # Clases helper
│   │   ├── Constants/                # Constantes del sistema
│   │   └── Models/                   # Modelos compartidos
│   │
│   └── SICAF.Services/               # Capa de Servicios Externos
│       ├── Email/                    # Servicios de email
│       ├── FileStorage/              # Servicios de almacenamiento
│       └── Interfaces/               # Interfaces de servicios
│
├── tests/
│   ├── SICAF.UnitTests/              # Pruebas unitarias
│   └── SICAF.IntegrationTests/       # Pruebas de integración
│
├── docs/                             # Documentación
├── .env                              # Variables de entorno
├── example.env                       # Ejemplo de configuración
├── .gitignore                        # Archivos ignorados por Git
├── README.md                         # Este archivo
└── SICAF.sln                         # Archivo de solución
```

## 🗄️ Esquema de Base de Datos

### Entidades Principales

#### Gestión de Usuarios
- **Users**: Información básica de usuarios
- **Roles**: Definición de roles del sistema
- **UserRoles**: Relación muchos a muchos entre usuarios y roles

#### Gestión Académica
- **Courses**: Cursos de aviación disponibles
- **Students**: Información específica de estudiantes
- **Instructors**: Datos de instructores
- **Enrollments**: Inscripciones de estudiantes en cursos

#### Sistema de Calificaciones
- **FlightPhases**: Fases de vuelo (Básica, Intermedia, Avanzada)
- **Grades**: Calificaciones de estudiantes
- **Missions**: Misiones y ejercicios prácticos
- **Evaluations**: Evaluaciones detalladas

#### Auditoría y Logs
- **AuditLogs**: Registro de actividades del sistema
- **SystemLogs**: Logs técnicos del sistema

## 🔧 Configuración

### Configuración de Desarrollo (appsettings.Development.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SICAF_DB;Uid=sicaf_user;Pwd=desarrollo123;"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/sicaf-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "Culture": {
    "DefaultCulture": "es-CO",
    "SupportedCultures": ["es-CO", "es-ES"]
  }
}
```

### Configuración de Producción

Para producción, usar variables de entorno seguras:

```bash
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=SICAF_PROD;Uid=prod_user;Pwd=password_segura;"
export ASPNETCORE_ENVIRONMENT="Production"
```

## 🧪 Pruebas

### Ejecutar Pruebas Unitarias
```bash
dotnet test tests/SICAF.UnitTests/
```

### Ejecutar Pruebas de Integración
```bash
dotnet test tests/SICAF.IntegrationTests/
```

### Ejecutar Todas las Pruebas con Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

### Generar Reporte de Cobertura
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"coverage/**/coverage.cobertura.xml" -targetdir:"coverage/report" -reporttypes:Html
```

## 🚀 Despliegue

### Despliegue en Azure

#### 1. Crear Recursos en Azure
```bash
# Crear grupo de recursos
az group create --name rg-sicaf --location "East US"

# Crear plan de App Service
az appservice plan create --name plan-sicaf --resource-group rg-sicaf --sku B1

# Crear Web App
az webapp create --name sicaf-app --resource-group rg-sicaf --plan plan-sicaf

# Crear MySQL Database
az mysql flexible-server create --name mysql-sicaf --resource-group rg-sicaf --admin-user sicafadmin --admin-password TuPassword123
```

#### 2. Configurar Variables de Entorno en Azure
```bash
az webapp config appsettings set --name sicaf-app --resource-group rg-sicaf --settings \
    ConnectionStrings:DefaultConnection="Server=mysql-sicaf.mysql.database.azure.com;Database=sicaf;Uid=sicafadmin;Pwd=TuPassword123;" \
    ASPNETCORE_ENVIRONMENT="Production"
```

### Despliegue Local con Docker

#### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SICAF.Web/SICAF.Web.csproj", "src/SICAF.Web/"]
RUN dotnet restore "src/SICAF.Web/SICAF.Web.csproj"
COPY . .
WORKDIR "/src/src/SICAF.Web"
RUN dotnet build "SICAF.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SICAF.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SICAF.Web.dll"]
```

#### docker-compose.yml
```yaml
version: '3.8'
services:
  sicaf-web:
    build: .
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sicaf-mysql;Database=sicaf;Uid=root;Pwd=sicaf123;
    depends_on:
      - sicaf-mysql

  sicaf-mysql:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: sicaf123
      MYSQL_DATABASE: sicaf
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql

volumes:
  mysql_data:
```

## 📚 Documentación Adicional

### Convenciones de Código

El proyecto sigue las convenciones establecidas en el archivo `.editorconfig`:

- **Nomenclatura**: PascalCase para clases y métodos, camelCase para variables
- **Campos privados**: Prefijo `_` (ej. `_repository`)
- **Campos estáticos privados**: Prefijo `s_` (ej. `s_logger`)
- **Async/Await**: Obligatorio para operaciones asíncronas
- **LINQ**: Preferido para consultas de datos

### Mejores Prácticas de Desarrollo

#### Servicios
```csharp
public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private static readonly ILogger s_logger = Log.ForContext<StudentService>();

    public async Task<StudentDto> GetStudentAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        return _mapper.Map<StudentDto>(student);
    }
}
```

#### Controladores
```csharp
[Area("Academic")]
public class StudentsController : Controller
{
    private readonly IStudentService _studentService;

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentService.GetStudentAsync(id);
        if (student == null)
            return NotFound();
            
        return View(student);
    }
}
```

## 🤝 Estándares de Commit

Usar [Conventional Commits](https://www.conventionalcommits.org/):

```
feat(academic): agregar módulo de calificaciones de vuelo
fix(auth): corregir validación de roles de instructor
docs(readme): actualizar instrucciones de instalación
test(services): agregar pruebas unitarias para StudentService
```