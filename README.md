# Sistema de Gestión de Agencia de Viajes

Proyecto completo con frontend .NET mediante Blazor WebAssembly, backend ASP.NET Core Web API, Entity Framework Core y PostgreSQL.

## Módulos
- Destinos: CRUD completo.
- Paquetes: CRUD completo y relación con destinos.
- Clientes: CRUD completo, documento y correo únicos.
- Reservas: CRUD completo, estados y relaciones con clientes y paquetes.

## Ejecución

### 1. Base de datos
Ejecute `database/agencia_viajes.sql` en PostgreSQL y cambie la contraseña en:
`backend/AgenciaViajes.Api/appsettings.json`.

También puede usar migraciones:
```powershell
cd backend/AgenciaViajes.Api
dotnet tool install --global dotnet-ef
dotnet ef migrations add Inicial
dotnet ef database update
```
No ejecute el script de creación y la migración inicial sobre la misma base.

### 2. Backend
```powershell
cd backend/AgenciaViajes.Api
dotnet restore
dotnet run
```
Swagger: `http://localhost:5100/swagger`

### 3. Frontend
En otra terminal:
```powershell
cd frontend/AgenciaViajes.Web
dotnet restore
dotnet run
```
Aplicación: `http://localhost:5200`

## Reglas
- No se elimina un destino con paquetes.
- No se elimina un paquete con reservas.
- No se elimina un cliente con reservas.
- No se permite duplicar documento o correo.
- La fecha de reserva no puede ser anterior a hoy.
- Estados: Pendiente, Confirmada, Cancelada y Completada.
