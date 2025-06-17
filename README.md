# MiMangaBot API

API REST para gestionar una colección de mangas.

## Características

- Gestión completa de mangas (CRUD)
- Búsqueda de mangas
- Listado de mangas populares
- API versionada (v1)
- Documentación con Swagger

## Tecnologías

- .NET 9.0
- ASP.NET Core Web API
- Swagger/OpenAPI
- C#

## Estructura del Proyecto

```
MiMangaBot/
├── Controllers/
│   └── V1/
│       └── MangaController.cs
├── Domain/
│   └── Entities/
│       └── Manga.cs
├── Services/
│   └── Features/
│       └── Mangas/
│           └── MangaService.cs
└── Program.cs
```

## Endpoints

- GET `/api/v1/manga` - Obtener todos los mangas
- GET `/api/v1/manga/{id}` - Obtener un manga por ID
- POST `/api/v1/manga` - Crear un nuevo manga
- PUT `/api/v1/manga/{id}` - Actualizar un manga existente
- DELETE `/api/v1/manga/{id}` - Eliminar un manga
- GET `/api/v1/manga/popular` - Obtener mangas populares
- GET `/api/v1/manga/search?query=termino` - Buscar mangas

## Cómo Ejecutar

1. Clona el repositorio
2. Navega al directorio del proyecto
3. Ejecuta `dotnet run`
4. Abre `http://localhost:5034/swagger` en tu navegador

## Requisitos

- .NET 9.0 SDK
- Visual Studio 2022 o VS Code 