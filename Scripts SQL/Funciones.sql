/*
===========================================
FUNCIONES
===========================================
*/

USE coworking;
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
--
-- Los parametros NO llevan la palabra IN: MariaDB la acepta en los procedimientos
-- pero no en las funciones (ERROR 1064). La sintaxis sin el modo es la que
-- funcionan en MySQL y en MariaDB, asi que se usa la misma en los dos.
CREATE FUNCTION fn_recurso_ocupado(
	p_id_recurso int,
	p_fecha_inicio datetime,
	p_fecha_fin datetime,
	p_id_reserva_excluida int
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
