CREATE DATABASE coworking;

USE coworking;

/*
===================================================
TABLAS
===================================================
*/

CREATE TABLE clientes(
	id int auto_increment primary key,
    nombre varchar(100) not null,
    apellido varchar(100) not null,
    dni int not null unique,
    telefono varchar(30) not null,
    email varchar(50) not null unique
);

CREATE TABLE tipo_recursos(
	id int auto_increment primary key,
    nombre_tipo_recurso varchar(100) not null,
    descripcion text
);

CREATE TABLE recursos(
	id int auto_increment primary key,
    id_tipo_recurso int not null,
    nombre varchar(100) not null,
    estado enum('Disponible', 'En Mantenimiento', 'Inhabilitado') default 'Disponible',
    ubicacion varchar(100) not null,
    precio_dia decimal(10,2) not null check(precio_dia > 0),
    
    foreign key(id_tipo_recurso) references tipo_recursos(id)
);

/*
	Esta tabla la cree para cumplir con la clase abstracta recurso y sus 3
    clases hijas, para que las diferencias sean reales y queden en base de datos.
    
    Aparte, el Procedimiento Almacenado va a tener que usar Transaction porque debe insertar en 'recursos'
    y en 'recursos_detalle'
*/
CREATE TABLE recursos_detalle(
	id_recurso int not null primary key,
    capacidad_personas int null,
    tiene_proyector bool null,
    numero_monitor int null,
    metros_cuadrados int null,
    
    foreign key(id_recurso) references recursos(id)
);

CREATE TABLE reservas(
	id int auto_increment primary key,
    id_cliente int not null,
    id_recurso int not null,
    fecha_inicio datetime not null check(fecha_inicio < fecha_fin),
    fecha_fin datetime not null check(fecha_fin > fecha_inicio),
    estado_reserva enum('Confirmada', 'Cancelada') default 'Confirmada',
    total_reserva decimal(10,2) not null,
    
    foreign key(id_cliente) references clientes(id),
    foreign key(id_recurso) references recursos(id)
);

/*
===================================================
DATOS DE PRUEBA
===================================================
*/

/* Clientes */
INSERT INTO clientes (nombre, apellido, dni, telefono, email) VALUES
('Juan', 'Perez', 40123456, '1145678901', 'juan.perez@example.com'),
('Maria', 'Gonzalez', 39654321, '1145678902', 'maria.gonzalez@example.com'),
('Carlos', 'Lopez', 42123987, '1145678903', 'carlos.lopez@example.com'),
('Ana', 'Martinez', 38765432, '1145678904', 'ana.martinez@example.com'),
('Luis', 'Rodriguez', 41111222, '1145678905', 'luis.rodriguez@example.com');

/* Tipos de recurso */
INSERT INTO tipo_recursos (nombre_tipo_recurso, descripcion) VALUES
('Sala', 'Salas de reunion con proyector y pizarra'),
('PuestoTrabajo', 'Puestos de trabajo individuales en area compartida'),
('OficinaPrivada', 'Oficinas privadas cerradas para equipos pequenos');

/* Recursos: Salas (tipo_recurso = 1) */
INSERT INTO recursos (id_tipo_recurso, nombre, estado, ubicacion, precio_dia) VALUES
(1, 'Sala A', 'Disponible', 'Planta baja', 5000.00),
(1, 'Sala B', 'Disponible', 'Planta baja', 4500.00),
(1, 'Sala C', 'En Mantenimiento', 'Primer piso', 5500.00);

/* Recursos: Puestos de trabajo (tipo_recurso = 2) */
INSERT INTO recursos (id_tipo_recurso, nombre, estado, ubicacion, precio_dia) VALUES
(2, 'Puesto 1', 'Disponible', 'Coworking principal', 1500.00),
(2, 'Puesto 2', 'Disponible', 'Coworking principal', 1500.00),
(2, 'Puesto 3', 'Disponible', 'Area silenciosa', 1800.00);

/* Recursos: Oficinas privadas (tipo_recurso = 3) */
INSERT INTO recursos (id_tipo_recurso, nombre, estado, ubicacion, precio_dia) VALUES
(3, 'Oficina 101', 'Disponible', 'Segundo piso', 12000.00),
(3, 'Oficina 102', 'Inhabilitado', 'Segundo piso', 15000.00);

/* Detalle de Salas (recursos 1, 2, 3) */
INSERT INTO recursos_detalle (id_recurso, capacidad_personas, tiene_proyector) VALUES
(1, 10, 1),
(2, 6, 0),
(3, 12, 1);

/* Detalle de Puestos de trabajo (recursos 4, 5, 6) */
INSERT INTO recursos_detalle (id_recurso, numero_monitor) VALUES
(4, 1),
(5, 2),
(6, 1);

/* Detalle de Oficinas privadas (recursos 7, 8) */
INSERT INTO recursos_detalle (id_recurso, metros_cuadrados) VALUES
(7, 25),
(8, 40);