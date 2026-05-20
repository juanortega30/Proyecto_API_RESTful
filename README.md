# LibraryAPI — API REST de Gestión de Biblioteca

API RESTful desarrollada en .NET 10 con Arquitectura Onion, patrón Repository y PostgreSQL.

## Tecnologías

- .NET 10
- Entity Framework Core + Npgsql (PostgreSQL)
- AutoMapper
- FluentValidation
- BCrypt.Net
- Swagger (Swashbuckle)
- xUnit + Moq + FluentAssertions

## Arquitectura

El proyecto implementa **Arquitectura Onion** con 4 capas:

```
LibraryAPI.Domain          → Entidades, interfaces, enums
LibraryAPI.Application     → Servicios, DTOs, validadores, AutoMapper
LibraryAPI.Infrastructure  → DbContext, repositorios, UnitOfWork
LibraryAPI.API             → Controllers, Middleware, Swagger
```

## Requisitos previos

- .NET 10 SDK
- PostgreSQL 14+
- dotnet-ef tools: `dotnet tool install --global dotnet-ef`

## Configuración

1. Clona el repositorio:
```bash
git clone https://github.com/TU_USUARIO/LibraryAPI.git
cd LibraryAPI
```

2. Configura la cadena de conexión en `src/LibraryAPI.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=library_db;Username=postgres;Password=TU_PASSWORD"
  }
}
```

3. Aplica las migraciones:
```bash
dotnet ef database update --project src/LibraryAPI.Infrastructure --startup-project src/LibraryAPI.API
```

4. Ejecuta el proyecto:
```bash
dotnet run --project src/LibraryAPI.API
```

5. Abre Swagger en: `http://localhost:5120/swagger`

## Endpoints principales

### Libros
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/books | Obtener todos los libros |
| GET | /api/books/{id} | Obtener libro por ID |
| GET | /api/books/search | Buscar libros (título, autor, género, paginación) |
| GET | /api/books/most-borrowed-by-category | Libro más prestado por categoría |
| POST | /api/books | Crear libro |
| PUT | /api/books/{id} | Actualizar libro |
| DELETE | /api/books/{id} | Eliminar libro |

### Usuarios
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/users | Obtener todos los usuarios |
| GET | /api/users/{id} | Obtener usuario por ID |
| POST | /api/users | Crear usuario |
| DELETE | /api/users/{id} | Eliminar usuario |

### Préstamos
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/loans | Obtener todos los préstamos |
| GET | /api/loans/{id} | Obtener préstamo por ID |
| GET | /api/loans/user/{userId} | Préstamos por usuario |
| GET | /api/loans/overdue | Préstamos vencidos |
| POST | /api/loans | Crear préstamo |
| PUT | /api/loans/{id}/return | Registrar devolución |

### Reservas
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/reservations | Obtener todas las reservas |
| POST | /api/reservations | Crear reserva |
| PUT | /api/reservations/{id}/cancel | Cancelar reserva |

## Pruebas unitarias

```bash
dotnet test src/LibraryAPI.Tests --verbosity normal
```

Resultado esperado: **24 pruebas pasadas, 0 fallos**

Las pruebas cubren:
- `BookServiceTests` — 6 pruebas
- `LoanServiceTests` — 4 pruebas  
- `UserServiceTests` — 5 pruebas
- `CreateBookValidatorTests` — 5 pruebas
- `CreateUserValidatorTests` — 4 pruebas

## Modelo de datos

Las entidades principales son:

- **Books** — Libros con título, ISBN, año de publicación
- **Authors** — Autores (relación muchos a muchos con libros)
- **Generos** — Categorías/géneros literarios
- **Copies** — Ejemplares físicos de cada libro
- **Users** — Usuarios con roles Admin y Reader
- **Loans** — Préstamos con cálculo de multas por retraso
- **Reservations** — Reservas de libros

## Decisiones de diseño

### Arquitectura Onion
Se eligió esta arquitectura para garantizar la separación de responsabilidades y que el dominio no dependa de ninguna tecnología externa. Esto facilita el testing y el mantenimiento.

### Patrón Repository + UnitOfWork
Centraliza el acceso a datos y garantiza que múltiples operaciones se guarden atómicamente.

### FluentValidation
Separación de la lógica de validación del controlador, manteniendo los DTOs limpios.

### AutoMapper
Elimina el código repetitivo de mapeo entre entidades y DTOs.

### PostgreSQL
Base de datos relacional robusta, ideal para las relaciones entre libros, autores y préstamos.
