/*
===========================================
PROCEDIMIENTOS ALMACENADOS CLIENTES
===========================================
*/

DELIMITER //
DROP PROCEDURE IF EXISTS sp_alta_cliente;
CREATE PROCEDURE sp_alta_cliente(
	in p_nombre varchar(100),
    in p_apellido varchar(100),
    in p_dni int,
    in p_telefono varchar(30),
    in p_email varchar(50)
)
BEGIN
	-- Validacion DNI
    IF EXISTS (SELECT dni FROM clientes WHERE dni = p_dni) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Ya existe un cliente con ese dni';
	END IF;
    
    -- Validacion EMAIL
    IF EXISTS (SELECT email FROM clientes WHERE email = p_email) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Ya existe un cliente con ese email';
    END IF;
    
	INSERT INTO clientes(nombre, apellido, dni, telefono, email)
    VALUES 
		(p_nombre, p_apellido, p_dni, p_telefono, p_email);
END //

DROP PROCEDURE IF EXISTS sp_modificar_cliente;
CREATE PROCEDURE sp_modificar_cliente(
	in p_id int,
    in p_nombre varchar(100),
    in p_apellido varchar(100),
    in p_dni int,
    in p_telefono varchar(30),
    in p_email varchar(50)
)
BEGIN
	-- Validacion Cliente
	IF NOT EXISTS (SELECT 1 FROM clientes WHERE id = p_id) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'No existe el cliente a modificar';
    END IF;
    
    -- Validacion DNI
    -- Se excluye el registro actual y se busca que no
    -- modifiquen el dni a otro ya existente
    IF EXISTS (SELECT 1 FROM clientes WHERE dni = p_dni AND id != p_id) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Ya existe otro cliente con ese dni';
    END IF;
	
    -- Validacion EMAIL
    IF EXISTS (SELECT 1 FROM clientes WHERE email = p_email AND id != p_id) then
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Ya existe otro cliente con ese email';
	END IF;
    
	UPDATE clientes
    SET
		nombre = p_nombre,
        apellido = p_apellido,
        dni = p_dni,
        telefono = p_telefono,
        email = p_email
	WHERE
		id = p_id;
END //

DELIMITER ;

/*
===========================================
PROCEDIMIENTOS ALMACENADOS RECURSOS
===========================================
*/

DELIMITER //
DROP PROCEDURE IF EXISTS sp_alta_recurso;
CREATE PROCEDURE sp_alta_recurso(
	in p_id_tipo_recurso int,
    in p_nombre varchar(100),
    in p_estado enum('Disponible', 'En Mantenimiento', 'Inhabilitado'),
    in p_ubicacion varchar(100),
    in p_precio_dia decimal(10,2),
    in p_capacidad_personas int,
    in p_tiene_proyector bool,
    in p_numero_monitor int,
    in p_metros_cuadrados int
)
BEGIN
/*
	Hay que usar LASTINSERTID, ya que debemos reutilizarlo en recursos_detalle,
    lo ideal seria filtrar con tipo_recursos para que el Procedimiento cargue con los datos que debe cada caso.
    Tambien usar transaction con rollback por las dudas porque toca dos tablas, o se hace todo o no se hace.
*/	
	DECLARE v_tipo varchar(100);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
		ROLLBACK;
        RESIGNAL;
    END;
    
	SELECT nombre_tipo_recurso INTO v_tipo
    FROM tipo_recursos
    WHERE id = p_id_tipo_recurso;
    
    IF v_tipo IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Tipo de recurso invalido';
    END IF;
    
    IF v_tipo = 'Sala' AND p_capacidad_personas IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sala requiere capacidad de personas';
    END IF;
    
    IF v_tipo = 'PuestoTrabajo' AND p_numero_monitor IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'PuestoTrabajo requiere numero de monitores';
    END IF;
    
    IF v_tipo = 'OficinaPrivada' AND p_metros_cuadrados IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'OficinaPrivada requiere metros cuadrados';
    END IF;
    
    START TRANSACTION;
		INSERT INTO recursos(id_tipo_recurso, nombre, estado, ubicacion, precio_dia)
        VALUES
			(p_id_tipo_recurso, p_nombre, p_estado, p_ubicacion, p_precio_dia);
            
		INSERT INTO recursos_detalle(id_recurso, capacidad_personas, tiene_proyector, numero_monitor, metros_cuadrados)
        VALUES
			(LAST_INSERT_ID(), p_capacidad_personas, p_tiene_proyector, p_numero_monitor, p_metros_cuadrados);
    COMMIT;
END //

DROP PROCEDURE IF EXISTS sp_modificar_recurso;
CREATE PROCEDURE sp_modificar_recurso(
	in p_id_recurso int,
    in p_nombre varchar(100),
    in p_estado enum('Disponible', 'En Mantenimiento', 'Inhabilitado'),
    in p_ubicacion varchar(100),
    in p_precio_dia decimal(10,2),
    in p_capacidad_personas int,
    in p_tiene_proyector bool,
    in p_numero_monitor int,
    in p_metros_cuadrados int
)
BEGIN
	DECLARE v_tipo varchar(100);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
		ROLLBACK;
        RESIGNAL;
    END;
    
	SELECT t.nombre_tipo_recurso INTO v_tipo
    FROM tipo_recursos t
    JOIN
		recursos r on r.id_tipo_recurso = t.id
	WHERE
		r.id = p_id_recurso;
    
    IF v_tipo IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Tipo de recurso invalido';
    END IF;
    
    IF v_tipo = 'Sala' AND p_capacidad_personas IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sala requiere capacidad de personas';
    END IF;
    
    IF v_tipo = 'PuestoTrabajo' AND p_numero_monitor IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'PuestoTrabajo requiere numero de monitores';
    END IF;
    
    IF v_tipo = 'OficinaPrivada' AND p_metros_cuadrados IS NULL THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'OficinaPrivada requiere metros cuadrados';
    END IF;
    
    START TRANSACTION;
    /*
		El update de recursos no debe modificar el id_tipo_recurso
    */
		UPDATE recursos
        SET
            nombre = p_nombre,
            estado = p_estado,
            ubicacion = p_ubicacion,
            precio_dia = p_precio_dia
        WHERE
			id = p_id_recurso;
            
		UPDATE recursos_detalle
        SET
			capacidad_personas = p_capacidad_personas,
            tiene_proyector = p_tiene_proyector,
            numero_monitor = p_numero_monitor,
            metros_cuadrados = p_metros_cuadrados
        WHERE
			id_recurso = p_id_recurso;
    COMMIT;
END //

DELIMITER ;

/*
===========================================
PROCEDIMIENTOS ALMACENADOS RESERVAS
===========================================
*/
DELIMITER //
DROP PROCEDURE IF EXISTS sp_alta_reserva;
CREATE PROCEDURE sp_alta_reserva(
	in p_id_cliente int,
    in p_id_recurso int,
    in p_fecha_inicio datetime,
    in p_fecha_fin datetime,
    in p_total_reserva decimal(10,2)
)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM clientes WHERE id = p_id_cliente) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El cliente no existe';
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM recursos WHERE id = p_id_recurso) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no existe';
    END IF;
    
    IF EXISTS (SELECT 1 FROM recursos WHERE id = p_id_recurso AND estado != 'Disponible') THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no esta disponible';
    END IF;
    
    IF p_fecha_inicio >= p_fecha_fin THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'La fecha de inicio debe ser anterior a la fecha de fin';
    END IF;
    
    /*
		Esto valida que no exista una reserva de un recurso en estado confirmada
        no choque con la fecha ingresada por parametro
    */
    IF EXISTS (
		SELECT 
			1 
		FROM 
			reservas 
		WHERE 
			id_recurso = p_id_recurso 
		AND estado_reserva = 'Confirmada'
        AND fecha_inicio <= p_fecha_fin
        AND fecha_fin >= p_fecha_inicio
	) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no esta disponible en la fecha ingresada';
    END IF;
    
	INSERT INTO reservas(id_cliente, id_recurso, fecha_inicio, fecha_fin, estado_reserva, total_reserva)
	VALUES (p_id_cliente, p_id_recurso, p_fecha_inicio, p_fecha_fin, 'Confirmada', p_total_reserva);
    
END //

DROP PROCEDURE IF EXISTS sp_modificar_reserva;
CREATE PROCEDURE sp_modificar_reserva(
	in p_id_reserva int,
    in p_id_recurso int,
    in p_fecha_inicio datetime,
    in p_fecha_fin datetime,
    in p_total_reserva decimal(10,2)
)
BEGIN
	IF NOT EXISTS (SELECT 1 FROM reservas WHERE id = p_id_reserva AND estado_reserva = 'Confirmada') THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'La reserva no existe o no se puede modificar';
	END IF;

    IF NOT EXISTS (SELECT 1 FROM recursos WHERE id = p_id_recurso) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no existe';
    END IF;
    
    IF EXISTS (SELECT 1 FROM recursos WHERE id = p_id_recurso AND estado != 'Disponible') THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no esta disponible';
    END IF;
    
    IF p_fecha_inicio >= p_fecha_fin THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'La fecha de inicio debe ser anterior a la fecha de fin';
    END IF;
    
    /*
		Esto valida que no exista una reserva de un recurso en estado confirmada
        no choque con la fecha ingresada por parametro
    */
    IF EXISTS (
		SELECT 
			1 
		FROM 
			reservas 
		WHERE 
			id_recurso = p_id_recurso 
		AND estado_reserva = 'Confirmada'
        AND fecha_inicio <= p_fecha_fin
        AND fecha_fin >= p_fecha_inicio
        AND id != p_id_reserva
	) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'El recurso no esta disponible en la fecha ingresada';
    END IF;
    
    UPDATE reservas
    SET
		id_recurso = p_id_recurso,
        fecha_inicio = p_fecha_inicio,
        fecha_fin = p_fecha_fin,
        total_reserva = p_total_reserva
	WHERE
		id = p_id_reserva;
END //

DROP PROCEDURE IF EXISTS sp_cancelar_reserva;
CREATE PROCEDURE sp_cancelar_reserva(
	in p_id_reserva int
)
BEGIN
	IF NOT EXISTS (SELECT 1 FROM reservas WHERE id = p_id_reserva AND estado_reserva = 'Confirmada') THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'La reserva no existe o ya esta cancelada';
    END IF;
    
    UPDATE reservas
	SET
		estado_reserva = 'Cancelada'
    WHERE
		id = p_id_reserva;
END //

DELIMITER ;