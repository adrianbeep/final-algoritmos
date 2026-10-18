/*
===========================================
FUNCIONES
===========================================
*/
DELIMITER //

CREATE FUNCTION fn_total_clientes()
RETURNS INT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	RETURN (SELECT count(*) FROM clientes);
END //

CREATE FUNCTION fn_reservas_por_finalizar()
RETURNS INT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	DECLARE v_reservas int;
    
    SELECT
		COUNT(*)
	INTO
		v_reservas
	FROM
		reservas
	WHERE
		estado_reserva = 'Confirmada' -- La reserva tiene que estar confirmada
        AND fecha_inicio <= now() -- Tiene que estar ya iniciada
        AND fecha_fin >= now(); -- No tiene que haber finalizado todavia
        
	RETURN v_reservas;
END //

CREATE FUNCTION fn_reservas_hoy()
RETURNS INT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	RETURN (SELECT COUNT(*) FROM reservas WHERE DATE(fecha_inicio) = CURDATE() AND estado_reserva = 'Confirmada');
END //

CREATE FUNCTION fn_recursos_disponibles()
RETURNS INT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	RETURN (SELECT COUNT(*) FROM recursos WHERE estado = 'Disponible');
END //

CREATE FUNCTION fn_recursos_reservados_hoy()
RETURNS INT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	DECLARE v_reservados int;
    
    SELECT
		COUNT(DISTINCT id_recurso)
	INTO
		v_reservados
	FROM
		reservas
	WHERE
		DATE(fecha_inicio) = CURDATE()
        AND estado_reserva = 'Confirmada';
        
	RETURN v_reservados;
END //
DELIMITER ;