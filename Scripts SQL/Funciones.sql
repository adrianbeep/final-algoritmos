/*
===========================================
FUNCIONES
===========================================
*/
DROP FUNCTION IF EXISTS fn_total_clientes;
DROP FUNCTION IF EXISTS fn_reservas_por_finalizar;
DROP FUNCTION IF EXISTS fn_reservas_hoy;
DROP FUNCTION IF EXISTS fn_recursos_disponibles;
DROP FUNCTION IF EXISTS fn_recursos_reservados_hoy;
DROP FUNCTION IF EXISTS fn_recurso_ocupado;

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
        AND DATE(fecha_inicio) <= CURDATE() -- Tiene que estar ya iniciada
        AND DATE(fecha_fin) >= CURDATE(); -- No tiene que haber finalizado todavia
        
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

-- Determina si un recurso ya tiene una reserva confirmada en el periodo indicado.
-- La usan los procedimientos almacenados y la aplicacion, para no duplicar la regla.
CREATE FUNCTION fn_recurso_ocupado(
	in p_id_recurso int,
	in p_fecha_inicio datetime,
	in p_fecha_fin datetime,
	in p_id_reserva_excluida int
)
RETURNS TINYINT
NOT DETERMINISTIC
READS SQL DATA
BEGIN
	DECLARE v_ocupado TINYINT;
    
    SELECT
		COUNT(*)
	INTO
		v_ocupado
	FROM
		reservas
	WHERE
		id_recurso = p_id_recurso
		AND estado_reserva = 'Confirmada' -- Solo las reservas confirmadas ocupan el recurso
        AND fecha_inicio <= p_fecha_fin
        AND fecha_fin >= p_fecha_inicio
        AND (p_id_reserva_excluida = 0 OR id != p_id_reserva_excluida); -- Al editar, la reserva no se superpone consigo misma
        
	IF v_ocupado > 0 THEN
		RETURN 1;
	END IF;
	
	RETURN 0;
END //
DELIMITER ;