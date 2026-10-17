# [Etapa 1] Crear README.md del proyecto

## Objetivo

Armar el `README.md` raiz del repositorio con la informacion basica del proyecto. Es la primera impresion que se lleva cualquier persona que entra al repo (incluido el profesor en la defensa).

## Secciones obligatorias

### 1. Nombre del proyecto
- Sistema de Gestion de Coworking y Reservas

### 2. Descripcion del problema
- Explicar brevemente que el sistema administra un espacio de coworking, sus recursos (salas, puestos, oficinas) y las reservas realizadas por clientes
- Mencionar que es un TP final integrador de Algoritmos II, Bases de Datos y Practicas Profesionalizantes II

### 3. Tecnologia a usar
- Lenguaje: C#
- Framework UI: Windows Forms (.NET Framework o .NET, segun lo que decidan)
- Base de datos: MySQL
- IDE: Visual Studio 2022 (o la version que usen)
- Conector: MySql.Data / MySqlConnector
- Control de versiones: Git + GitHub

### 4. Desarrolladores involucrados
- Lista de los integrantes del grupo (apellido y nombre completos, como pide la consigna de presentacion)
- Formato: lista con el nombre de cada uno
- Ejemplo:
  - Aroca, Adrian (Profesor)
  - Apellido, Nombre
  - Apellido, Nombre

### 5. Fecha
- Ciclo lectivo: 2026
- Ultima actualizacion: [completar al hacer el PR]

### 6. Como clonar el repositorio
- Comando `git clone` con la URL del repo
- Requisitos previos: .NET SDK, MySQL Server, Visual Studio
- Pasos posteriores:
  - Abrir la solucion en Visual Studio
  - Crear la base de datos ejecutando `Scripts SQL/Coworking_db.sql` en MySQL Workbench
  - Configurar la cadena de conexion (cuando se implemente en Etapa 6)
  - Compilar y ejecutar

## Criterio de pase

- [ ] El README se ve bien renderizado en GitHub
- [ ] Todas las secciones obligatorias estan completas (sin placeholders visibles)
- [ ] Los nombres de los desarrolladores coinciden con la portada del trabajo final
- [ ] Las instrucciones de clonacion son suficientes para que alguien sin contexto pueda correr el proyecto

## Archivos a crear/modificar

- `README.md` (sobreescribir el actual)
