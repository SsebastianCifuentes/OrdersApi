# OrdersApi

API REST para la gestión de órdenes de compra y productos, desarrollada en ASP.NET Core (.NET 9) con Entity Framework Core y SQLite.

## Características

- CRUD de productos.
- CRUD de órdenes de compra, incluyendo productos asociados.
- Cálculo automático de descuentos en órdenes:
  - 10% si el total supera $500.
  - 5% adicional si hay más de 5 productos distintos.
- Paginación en la consulta de órdenes.
- Documentación interactiva con Swagger.

## Requisitos

- [.NET 9 SDK]
- [SQLite]

## Instalación

1. Clona el repositorio:
   ```sh
   git clone https://github.com/SsebastianCifuentes/OrdersApi.git
   cd OrdersApi
   ```

2. Restaura los paquetes NuGet:
   ```sh
   dotnet restore
   ```

3. Aplica las migraciones y crea la base de datos:
   ```sh
   dotnet ef database update
   ```

4. Ejecuta la API:
   ```sh
   dotnet run
   ```

5. Accede a la documentación Swagger en:
   ```
   https://localhost:5296/swagger
   ```

## Endpoints principales

- `POST /api/productos` — Crear producto
- `GET /api/productos` — Listar productos
- `POST /api/ordenes` — Crear orden de compra
- `GET /api/ordenes` — Listar órdenes (paginado)
- `GET /api/ordenes/{id}` — Obtener orden por ID
- `PUT /api/ordenes/{id}` — Actualizar orden
- `DELETE /api/ordenes/{id}` — Eliminar orden

## Notas

- El proyecto usa SQLite por simplicidad, pero se puede cambiar a SQL Server LocalDB en `Program.cs` y `appsettings.json` si se prefiere.
- Swagger está habilitado en entorno de desarrollo.

**Autor:** Sebastian Cifuentes
