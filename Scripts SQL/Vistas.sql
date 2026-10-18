/*
===========================================
VISTAS
===========================================
*/
DROP VIEW IF EXISTS vw_reservas_completas;
CREATE VIEW vw_reservas_completas AS
SELECT
	r.id as id_reserva,
    r.fecha_inicio, 
    r.fecha_fin, 
    r.estado_reserva, 
    r.total_reserva,
	concat(c.nombre, ' ', c.apellido) as cliente_nombre, 
    c.dni,
    re.nombre as recurso_nombre, 
    re.ubicacion as recurso_ubicacion, 
    tr.nombre_tipo_recurso
FROM
	reservas r
INNER JOIN
	clientes c on c.id = r.id_cliente
INNER JOIN
	recursos re on re.id = r.id_recurso
INNER JOIN
	tipo_recursos tr on tr.id = re.id_tipo_recurso;
    
DROP VIEW IF EXISTS vw_recursos_estado;
CREATE VIEW vw_recursos_estado AS
SELECT
	r.id as id_recurso,
    r.nombre as recurso_nombre,
    r.ubicacion as recurso_ubicacion,
    r.estado,
    r.precio_dia,
    tr.nombre_tipo_recurso,
    (SELECT
		count(id)
	FROM
		reservas
	WHERE
		estado != 'Cancelada' AND
        fecha_fin >= NOW() AND
        r.id = id) as reservas_activas
FROM
	recursos r
INNER JOIN
	tipo_recursos tr on tr.id = r.id_tipo_recurso;