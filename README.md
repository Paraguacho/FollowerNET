# FollowerNET

FollowerNET es una aplicación web para gestionar la relación entre egresados y responsables de programas educativos. La plataforma permite que los egresados registren su información, gestionen su perfil y soliciten acceso; mientras que los responsables revisan solicitudes, autorizan registros y consultan indicadores del programa.

Este documento describe el proyecto sin alterar la funcionalidad actual ni reorganizar la lógica existente. Su propósito es documentar la arquitectura, módulos, flujo de trabajo y estructura del repositorio para facilitar mantenimiento y futuras ampliaciones.

## 1. Objetivo del sistema

El sistema busca apoyar la trazabilidad y seguimiento de egresados mediante:

- registro y validación de solicitudes de acceso
- perfil de egresado con información personal, académica y laboral
- relación entre egresados y programas educativos
- autorizaciones de responsables de programa
- panel de indicadores para evaluar la población egresada
- administración de datos personalizados por programa

## 2. Tecnologías utilizadas

- ASP.NET Core 10
- Blazor Server
- MVC
- Entity Framework Core
- MySQL con proveedor de EF Core para MySQL
- Bootstrap + CSS personalizado
- Autenticación basada en Claims y `ProtectedSessionStorage`
- BCrypt.Net-Next para manejo de contraseñas

## 3. Estructura del repositorio

```text
FollowerNET/
├── Follower/
│   ├── App.razor
│   ├── Program.cs
│   ├── _Imports.razor
│   ├── App.razor
│   ├── Auth/
│   │   └── CustomAuthStateProvider.cs
│   ├── Controllers/
│   │   └── HomeController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── DTOs/
│   │   ├── LoginDto.cs
│   │   └── RegistroEgresadoDto.cs
│   ├── Migrations/
│   ├── Models/
│   │   ├── CampoPersonalizado.cs
│   │   ├── Egresado.cs
│   │   ├── ExperienciaLaboral.cs
│   │   ├── FormacionAcademica.cs
│   │   ├── OpcionCatalogoCampo.cs
│   │   ├── PerfilAcademico.cs
│   │   ├── ProgramaEducativo.cs
│   │   ├── ProgramaResponsable.cs
│   │   ├── Rol.cs
│   │   ├── SeccionPersonalizada.cs
│   │   ├── SituacionLaboral.cs
│   │   ├── Usuario.cs
│   │   ├── ValorCampoEgresado.cs
│   │   └── ...
│   ├── Pages/
│   │   ├── Index.razor
│   │   ├── Login.razor
│   │   ├── Registro.razor
│   │   ├── Egresado/
│   │   └── Responsable/
│   ├── Services/
│   │   ├── IEgresadoService.cs
│   │   └── EgresadoService.cs
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   ├── NavBar.razor
│   │   ├── NavBarResponsable.razor
│   │   └── IndicatorPanelHeading.razor
│   ├── wwwroot/
│   ├── appsettings.Development.json
│   ├── Follower.csproj
│   └── Properties/
│       └── launchSettings.json
├── script_follower.sql
├── script_follower2.sql
├── Follower.slnx
├── .gitignore
├── .gitattributes
└── README.md
```

## 4. Punto de entrada

La aplicación inicia en:

- [Follower/Program.cs](Follower/Program.cs)

En `Program` se registra:

- `ApplicationDbContext`
- MVC
- Blazor Server
- autorización
- `CustomAuthStateProvider`
- servicio `IEgresadoService`

También se configuran rutas, mapeo de Blazor y MVC, además de la conexión a MySQL.

## 5. Configuración de base de datos

La aplicación usa Entity Framework Core con MySQL. La conexión se configura en `Program.cs` mediante `AddDbContext` y `UseMySql`.

El proyecto incluye scripts SQL en:

- [script_follower.sql](script_follower.sql)
- [script_follower2.sql](script_follower2.sql)

Estos scripts contienen la base de datos y estructura inicial del proyecto. El contexto principal para el modelo se define en:

- [Follower/Data/ApplicationDbContext.cs](Follower/Data/ApplicationDbContext.cs)

### DbContext principal

`ApplicationDbContext` expone los conjuntos principales:

- `Roles`
- `Usuarios`
- `ProgramasEducativos`
- `ProgramasResponsables`
- `Egresados`
- `SeccionesPersonalizadas`
- `CamposPersonalizados`
- `OpcionesCatalogoCampos`
- `ValoresCamposEgresados`
- `PerfilesAcademicos`
- `SituacionesLaborales`
- `ExperienciasLaborales`
- `FormacionesAcademicas`

Entre los ajustes más importantes del modelo están:

- correo único por usuario
- relación uno a uno entre `Usuario` y `Egresado`
- relación uno a muchos entre `ProgramaEducativo` y `Egresado`
- restricciones de unicidad entre programa y responsable
- cascadas en relaciones de secciones, campos y valores

## 6. Modelo de dominio

### Usuario

Archivo:

- [Follower/Models/Usuario.cs](Follower/Models/Usuario.cs)

Contiene:

- `IdUsuario`
- `Correo`
- `ContrasenaHash`
- `IdRol`
- `Rol`
- `Egresado`
- `ProgramasAsignados`

### Rol

Archivo:

- [Follower/Models/Rol.cs](Follower/Models/Rol.cs)

Define los permisos o categorías del sistema. En la práctica, el proyecto usa el rol de `ResponsablePrograma` y el rol de egresado.

### Egresado

Archivo:

- [Follower/Models/Egresado.cs](Follower/Models/Egresado.cs)

Es la entidad principal del flujo de negocio. Guarda:

- datos personales
- programa educativo
- estado de la solicitud (`Pendiente`, `Aprobado`, `Rechazado`)
- país, estado, ciudad
- privacidad del perfil
- fecha de nacimiento, egreso y resolución
- motivo de rechazo
- perfil académico
- situación laboral
- experiencias laborales
- formaciones académicas
- valores de campos personalizados

### ProgramaEducativo

Archivo:

- [Follower/Models/ProgramaEducativo.cs](Follower/Models/ProgramaEducativo.cs)

Representa cada programa sobre el que se trabaja. Tiene:

- `Siglas`
- `NombrePrograma`
- lista de responsables
- lista de secciones personalizadas

### ProgramaResponsable

Archivo:

- [Follower/Models/ProgramaResponsable.cs](Follower/Models/ProgramaResponsable.cs)

Relaciona a un `Usuario` con un `ProgramaEducativo` para representar la responsabilidad institucional.

### PerfilAcademico

Archivo:

- [Follower/Models/PerfilAcademico.cs](Follower/Models/PerfilAcademico.cs)

Incluye:

- ORCID
- Google Scholar
- ResearchGate
- Academia.edu
- Scopus ID
- LinkedIn

### SituacionLaboral

Archivo:

- [Follower/Models/SituacionLaboral.cs](Follower/Models/SituacionLaboral.cs)

Representa el estado actual de empleo o actividad profesional del egresado, junto con indicadores de cambios relevantes como:

- nuevo empleo
- promoción
- cambio de institución
- nuevas responsabilidades
- incorporación a posgrado
- emprendimiento propio

### ExperienciaLaboral

Archivo:

- [Follower/Models/ExperienciaLaboral.cs](Follower/Models/ExperienciaLaboral.cs)

Guarda:

- institución
- puesto
- sector
- fechas de inicio y fin
- indicador de empleo actual
- actividades principales
- relación con la formación

### FormacionAcademica

Archivo:

- [Follower/Models/FormacionAcademica.cs](Follower/Models/FormacionAcademica.cs)

Guarda información sobre estudios, maestrías, doctorados, becas y apoyos:

- nombre del programa
- grado
- institución
- país
- estado del programa
- fechas
- si es formación actual
- si recibió beca
- tipo de apoyo
- monto asignado

## 7. Autenticación y autorización

La autenticación se implementa en:

- [Follower/Auth/CustomAuthStateProvider.cs](Follower/Auth/CustomAuthStateProvider.cs)

Se usa `AuthenticationStateProvider` personalizado con `ProtectedSessionStorage` para persistir sesión del usuario. La sesión guarda:

- `userId`
- `userRole`
- `loginTimestamp`
- `userName`

La sesión expira si pasan más de 12 horas. Al recuperar el estado del usuario, se construyen `Claims` con:

- `ClaimTypes.NameIdentifier`
- `ClaimTypes.Role`
- `ClaimTypes.Name`

Las pantallas y rutas protegidas usan el atributo `[Authorize]` o `[Authorize(Roles = "ResponsablePrograma")]`.

## 8. Flujos de usuario

### 8.1 Registro de egresado

Vista principal:

- [Follower/Pages/Registro.razor](Follower/Pages/Registro.razor)

Secuencia:

1. El usuario llena el formulario de registro.
2. El sistema valida si el correo ya existe.
3. Se crea un `Usuario` con rol de egresado.
4. Se crea un `Egresado` asociado al `Usuario`.
5. El estado de la solicitud queda en `Pendiente`.
6. Se confirma la solicitud y la pantalla muestra un mensaje de éxito.

### 8.2 Inicio de sesión

Vista principal:

- [Follower/Pages/Login.razor](Follower/Pages/Login.razor)

Secuencia:

1. Se busca el usuario por correo.
2. Se verifica la contraseña.
3. Se rechaza si la cuenta está pendiente o rechazada.
4. Se construye el nombre del usuario con su información y se inicia la sesión.
5. Se redirige según el rol.

### 8.3 Perfil del egresado

Vista principal:

- [Follower/Pages/Egresado/Perfil.razor](Follower/Pages/Egresado/Perfil.razor)

Subsecciones:

- [Follower/Pages/Egresado/SeccionesPerfil/InformacionGeneral.razor](Follower/Pages/Egresado/SeccionesPerfil/InformacionGeneral.razor)
- [Follower/Pages/Egresado/SeccionesPerfil/Trayectoria.razor](Follower/Pages/Egresado/SeccionesPerfil/Trayectoria.razor)

El egresado puede gestionar:

- datos generales
- contacto
- ubicación
- perfiles académicos
- contactos de red
- información laboral y educativa
- historial y trayectoria

### 8.4 Panel de indicadores del responsable

Vista principal:

- [Follower/Pages/Responsable/PanelIndicadores.razor](Follower/Pages/Responsable/PanelIndicadores.razor)

La lógica del panel:

- identifica al responsable logueado
- obtiene los programas asignados
- carga los egresados aprobados de esos programas
- filtra por generación
- calcula métricas de:
  - total de egresados
  - perfiles actualizados
  - porcentaje de actualizaciones
  - distribución por generación
  - situación laboral
  - sectores de empleo
  - doctorados
  - becas

### 8.5 Solicitudes de ingreso

Vista:

- [Follower/Pages/Responsable/SolicitudesIngreso.razor](Follower/Pages/Responsable/SolicitudesIngreso.razor)

Permite:

- ver solicitudes pendientes
- validar el perfil del egresado
- aprobar o rechazar la inscripción
- dejar un motivo de rechazo si aplica

## 9. Servicios de negocio

Servicio principal:

- [Follower/Services/EgresadoService.cs](Follower/Services/EgresadoService.cs)

Responsabilidades:

- recuperar un egresado por IdUsuario
- cargar dependencias relacionadas
- actualizar perfil académico y situación laboral
- eliminar experiencias o formaciones
- actualizar fecha de última modificación

## 10. Vistas y UX

La UI se compone principalmente de páginas Razor + componentes reutilizables en `Shared`:

- [Follower/Shared/MainLayout.razor](Follower/Shared/MainLayout.razor)
- [Follower/Shared/NavBar.razor](Follower/Shared/NavBar.razor)
- [Follower/Shared/NavBarResponsable.razor](Follower/Shared/NavBarResponsable.razor)
- [Follower/Shared/IndicatorPanelHeading.razor](Follower/Shared/IndicatorPanelHeading.razor)

El proyecto usa una estética institucional con tonos azules y paneles de administración. La navegación cambia según el rol del usuario.

## 11. Modelado de datos personalizado

El proyecto incluye soporte para secciones personalizadas y campos dinámicos. Esto permite adaptar la información que cada programa educativo desea capturar.

Entidades relacionadas:

- [Follower/Models/SeccionPersonalizada.cs](Follower/Models/SeccionPersonalizada.cs)
- [Follower/Models/CampoPersonalizado.cs](Follower/Models/CampoPersonalizado.cs)
- [Follower/Models/OpcionCatalogoCampo.cs](Follower/Models/OpcionCatalogoCampo.cs)
- [Follower/Models/ValorCampoEgresado.cs](Follower/Models/ValorCampoEgresado.cs)

Esto da flexibilidad para personalizar perfiles por programa, sin perder la estructura base del sistema.

## 12. Requisitos para ejecutar el proyecto

### Requerimientos

- .NET SDK compatible con `net10.0`
- MySQL
- entorno de desarrollo con Visual Studio / VS Code / dotnet CLI

### Comandos básicos

Desde la raíz del repositorio:

```bash
dotnet restore

dotnet build

dotnet run --project Follower/Follower.csproj
```

## 13. Observaciones importantes sobre el estado actual

El proyecto ya compila correctamente con la solución actual. Durante la validación se observaron avisos de nulabilidad y algunos detalles de limpieza, pero no hubo errores de compilación.

Se recomienda revisar con atención:

- uso de contraseñas en login
- estandarización de autenticación con un mecanismo más seguro para producción
- revisión de mensajes y avisos de nulabilidad
- validación adicional de rutas y permisos en pantallas de administración

## 14. Resumen ejecutivo

FollowerNET es una solución web enfocada en el seguimiento de egresados y la gestión del vínculo institucional entre egresados y programas educativos. El sistema integra registro, aprobación, perfilización, historial académico/laboral y panel estadístico para responsables.

La documentación actual busca describir el proyecto tal como está construido, conservando su comportamiento y evitando mover su funcionalidad actual.

## 15. Módulos principales

- Módulo de autenticación
- Módulo de registro y solicitud
- Módulo de perfil de egresado
- Módulo de programas y responsables
- Módulo de indicadores
- Módulo de configuración y personalización de campos

## 16. Conclusión

El proyecto presenta una base sólida para gestión de egresados con un enfoque institucional y analítico. Su estructura es clara, su dominio está bien representado en entidades y su UX está diseñada para dos actores principales: egresado y responsable.

La intención de esta documentación es servir como punto de partida para mantenimiento, ampliación y onboarding del equipo.
