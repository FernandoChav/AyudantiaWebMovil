# AYUDANTIA IDWM - Backend E-Commerce

## 📄 Descripción del Proyecto

Este sistema corresponde al backend de una plataforma de comercio electrónico desarrollada en el contexto del **Taller de Introducción al Desarrollo Web/Móvil (IDWM)** de la carrera de **Ingeniería Civil en Computación e Informática**.  
Permite gestionar productos, usuarios, pedidos y carrito de compras mediante una API REST desarrollada en ASP.NET Core.

---

## 👥 Integrantes del equipo

- **Nombre:** Fernando Chavez 
- **Correo:** [fernando.chavez@alumnos.ucn.cl](mailto:fernando.chavez@alumnos.ucn.cl)  
- **RUT:** 21.180.530 - 7 

*(Agrega los datos de los demás integrantes si corresponde)*

---

## 🛠 Tecnologías utilizadas

- [.NET 9](https://dotnet.microsoft.com/en-us/download) (ASP.NET Core Web API)
- Entity Framework Core (SQLite)
- Identity + JWT Bearer para autenticación
- Cloudinary para gestión de imágenes
- Serilog para logging estructurado
- Postman (para pruebas manuales y automatizadas)
- Husky (.NET) para validaciones automáticas

---

## ⚙️ Instalación y Ejecución

### 🔑 Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQLite](https://www.sqlite.org/download.html)

### 🧪 Pasos

1. Clonar el repositorio y acceder a el proyecto:

   ```bash
   git clone https://github.com/FernandoChav/AyudantiaWebMovil.git
   cd Ayudantia
   ```
2. Restaurar dependencias:
   ```bash
   dotnet restore
   ```
3. Crear y aplicar migraciones:
  ```bash
   dotnet ef database update
  ```
4. Ejecutar la aplicación:
   ```bash
   dotnet run
  
## 🌳 Estructura de Ramas

- `main`: versión estable y final de entrega
- `dev`: integración de nuevas funcionalidades
- `features/nombre`: ramas para cada funcionalidad independiente



## 🔗 Endpoints principales

| Recurso   | Método | Ruta                        | Rol requerido |
|-----------|--------|-----------------------------|----------------|
| Productos | GET    | `/products`                 | Público        |
| Productos | POST   | `/products/create`          | Admin          |
| Usuarios  | POST   | `/auth/register`            | Público        |
| Usuarios  | POST   | `/auth/login`               | Público        |
| Perfil    | GET    | `/user/profile`             | User           |
| Carrito   | POST   | `/basket?productId=X...`    | User           |
| Pedidos   | POST   | `/orders`                   | User           |

> *(Ver colección Postman para el detalle completo de pruebas y flujos)*

---

## 🔐 Variables de entorno

Agrega estas claves en tu archivo `appsettings.json` o como variables de entorno:

```json
"JWT": {
  "SignInKey": "<tu-clave-secreta>",
  "Issuer": "https://localhost:7088",
  "Audience": "https://localhost:7088"
},
"Cloudinary": {
  "CloudName": "<nombre>",
  "ApiKey": "<api-key>",
  "ApiSecret": "<api-secret>"
},
"CorsSettings": {
  "AllowedOrigins": ["http://localhost:3000"],
  "AllowedHeaders": ["Content-Type", "Authorization"],
  "AllowedMethods
```
---

## 🧪 Pruebas con Postman

Se incluye una colección Postman organizada por recurso (`Products`, `User`, `Basket`, `Order`), que permite:

- Registrar y autenticar usuarios
- Probar flujos como usuario y administrador
- Crear productos y subir imágenes a Cloudinary
- Filtrar, ordenar y paginar productos y usuarios
- Agregar y quitar productos del carrito
- Crear pedidos y consultar su historial

### 🔧 Variables utilizadas

| Variable    | Descripción                          |
|-------------|--------------------------------------|
| `{{url}}`   | URL base del backend (ej: https://localhost:7088) |
| `{{token}}` | Token Bearer obtenido tras el login |

> 📎 Importa la colección `Punto de prueba.postman_collection.json` en Postman y configura las variables antes de testear.

---

### 🧭 Flujo automatizado sugerido

| Paso | Descripción                                | Método | Ruta                        | Requiere token |
|------|--------------------------------------------|--------|-----------------------------|----------------|
| 1    | Registro de nuevo usuario                  | POST   | `/auth/register`            | ❌              |
| 2    | Intento de registro duplicado              | POST   | `/auth/register`            | ❌              |
| 3    | Login y obtención de token                 | POST   | `/auth/login`               | ❌              |
| 4    | Obtener perfil del usuario autenticado     | GET    | `/user/profile`             | ✅              |
| 5    | Actualizar dirección de envío              | PUT    | `/user/address`             | ✅              |
| 6    | Obtener listado de productos públicos      | GET    | `/products`                 | ❌              |
| 7    | Agregar producto al carrito                | POST   | `/basket?productId=X`       | ✅              |
| 8    | Ver contenido del carrito                  | GET    | `/basket`                   | ✅              |
| 9    | Crear pedido desde el carrito              | POST   | `/orders`                   | ✅              |
| 10   | Ver historial de pedidos                   | GET    | `/orders`                   | ✅              |
| 11   | Ver detalle de pedido por ID               | GET    | `/orders/{id}`              | ✅              |

---

## 💬 Convenciones de Commit

Se utilizó el estándar **Conventional Commits** para mejorar la trazabilidad del código:

- `feat:` nueva funcionalidad
- `fix:` corrección de errores
- `refactor:` mejoras internas sin cambio de lógica
- `docs:` actualizaciones en documentación
- `test:` adición o mejora de pruebas

---

## ✅ Validaciones automáticas

Se configuraron Git Hooks con **Husky (.NET)** para ejecutar validaciones previas al commit:

```bash
dotnet format
dotnet build
dotnet list ./Ayudantia/Ayudantia.csproj package --outdated
dotnet ef migrations script
```
