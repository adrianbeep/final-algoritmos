/*
===========================================
TRIGGERS
===========================================
*/
-- MySql limita los triggers a una accion, por ej sql server permite hacer un trigger
-- para insert y para update.
DELIMITER //
DROP TRIGGER IF EXISTS trg_auditoria_reservas_insert;
CREATE TRIGGER trg_auditoria_reservas_insert
AFTER INSERT ON reservas -- indica que se activa despues de un insert en tabla reservas
FOR EACH ROW -- por cada fila sino el default es por sentencia,
-- el peligro de no dejarlo explicito es que una unica sentencia
-- puede modificar varias filas
BEGIN 
	INSERT INTO auditoria_reservas(id_reserva, accion, detalle)
    VALUES 
		( 
		NEW.id, 
		'INSERT', 
		CONCAT('Reserva creada. Cliente: ', NEW.id_cliente,
			   ', Recurso: ', NEW.id_recurso,
			   ', Inicio: ', NEW.fecha_inicio,
			   ', Fin: ', NEW.fecha_fin,
			   ', Total: ', NEW.total_reserva)
		);
END //

DROP TRIGGER IF EXISTS trg_auditoria_reservas_update;
CREATE TRIGGER trg_auditoria_reservas_update
AFTER UPDATE ON reservas
FOR EACH ROW
BEGIN
	IF OLD.estado_reserva = 'Confirmada' AND NEW.estado_reserva = 'Cancelada' THEN
		INSERT INTO auditoria_reservas(id_reserva, accion, detalle)
		VALUES 
			(
				NEW.id,
                'CANCEL',
                CONCAT('Reserva cancelada. Estado anterior: ', OLD.estado_reserva,
                   ', Estado nuevo: ', NEW.estado_reserva,
                   ', Cliente: ', OLD.id_cliente,
                   ', Recurso: ', OLD.id_recurso)
			);
	END IF;
END //

DELIMITER ;