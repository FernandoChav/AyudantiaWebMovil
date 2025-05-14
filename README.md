AYUDANTIA IDWM - Backend E-Commerce

Descripción del Proyecto

Este sistema corresponde al backend de una plataforma de comercio electrónico desarrollada en el contexto del Taller de Introducción al Desarrollo Web/Móvil (IDWM) de la carrera de Ingeniería Civil en Computación e Informática. Permite gestionar productos, usuarios, pedidos y carrito de compras mediante una API REST desarrollada en ASP.NET Core.

Integrantes del equipo

Nombre: Samuel Fuentes

Correo: samuel.fuentes@ejemplo.com

RUT: 12.345.678-9

(Agrega los datos de los demás integrantes si corresponde)

Tecnologías utilizadas

.NET 9 (ASP.NET Core Web API)

Entity Framework Core (SQLite)

Identity + JWT Bearer para autenticación

Cloudinary para gestión de imágenes

Serilog para logging estructurado

Postman (para pruebas)

Husky (.NET) para hooks de validación

Instalación y Ejecución

Requisitos previos

.NET 9 SDK

SQLite

Pasos

Clonar el repositorio:

git clone https://github.com/tuusuario/AYUDANTIAIDWM.git

Restaurar dependencias:

dotnet restore

Crear y aplicar migraciones:

dotnet ef database update

Ejecutar la aplicación:

dotnet run --project ./Ayudantia/Ayudantia.csproj

La API quedará disponible en:

https://localhost:7088

Estructura de Ramas

main: versión estable y final de entrega

dev: integración de nuevas funcionalidades

features/nombre: ramas para cada funcionalidad independiente

Endpoints principales

Recurso

Método

Ruta

Rol Requerido

Productos

GET

/products

Público

Productos

POST

/products/create

Admin

Usuarios

POST

/auth/register

Público

Usuarios

POST

/auth/login

Público

Perfil

GET

/user/profile

User

Carrito

POST

/basket?productId=X...

User

Pedidos

POST

/orders

User

(Ver Postman Collection para detalle completo de pruebas)

Variables de entorno

Configura las siguientes claves en appsettings.json o mediante entorno:

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
  "AllowedMethods": ["GET", "POST", "PUT", "DELETE"]
}

Pruebas con Postman

Se incluye una colección Postman organizada por recurso (Products, User, Basket, Order), que permite:

Registrar, autenticar y probar flujos como usuario y administrador

Agregar productos y subir imágenes

Filtrar, ordenar y paginar productos y usuarios

Gestionar el carrito y crear pedidos

Ver el historial y detalle de pedidos

Variables utilizadas:

{{url}}: URL base del backend (ej. https://localhost:7088/)

{{token}}: token Bearer obtenido tras login

Recuerda importar la colección Punto de prueba.postman_collection.json en tu Postman y configurar las variables adecuadas antes de comenzar a testear.

Convenciones de Commit

Se utilizó el estándar Conventional Commits:

feat: nueva funcionalidad

fix: corrección de errores

refactor: mejoras internas

docs: documentación

Validaciones automáticas

Se configuraron Git Hooks con Husky (.NET) para ejecutar:

dotnet format

dotnet build

dotnet list ... --outdated

dotnet ef migrations script

Licencia

Este proyecto es parte de una entrega universitaria y no debe usarse con fines comerciales sin autorización.
