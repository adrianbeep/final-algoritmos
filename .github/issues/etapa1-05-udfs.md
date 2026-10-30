# [Etapa 1] Crear funciones almacenadas (UDFs) del dashboard

## Objetivo

Crear las 5 funciones definidas por el usuario que alimentan las 4 tarjetas del dashboard. Cada UDF tiene un uso real y visible en la UI, ninguna es relleno.

## Entregables

### UDFs a crear

- [ ] `fn_total_clientes() RETURNS INT` — total de clientes registrados
  ```sql
  SELECT COUNT(*) FROM clientes;
  ```
- [ ] `fn_reservas_por_finalizar() RETURNS INT` — reservas confirmadas que aun no terminaron
  ```sql
  SELECT COUNT(*) FROM reservas
  WHERE estado_reserva = 'Confirmada' AND fecha_fin >= NOW();
  ```
- [ ] `fn_reservas_hoy() RETURNS INT` — reservas que empiezan hoy
  ```sql
  SELECT COUNT(*) FROM reservas
  WHERE DATE(fecha_inicio) = CURDATE() AND estado_reserva = 'Confirmada';
  ```
- [ ] `fn_recursos_disponibles() RETURNS INT` — recursos en estado `Disponible` (inventario bookable)
  ```sql
  SELECT COUNT(*) FROM recursos WHERE estado = 'Disponible';
  ```
- [ ] `fn_recursos_reservados_hoy() RETURNS INT` — recursos DISTINTOS con al menos una reserva hoy
  ```sql
  SELECT COUNT(DISTINCT id_recurso) FROM reservas
  WHERE estado_reserva = 'Confirmada' AND DATE(fecha_inicio) = CURDATE();
  ```

## Uso en el dashboard (Etapa 2)

- [ ] Tarjeta "Total clientes" → consume `fn_total_clientes`
- [ ] Tarjeta "Reservas por finalizar" → consume `fn_reservas_por_finalizar`
- [ ] Tarjeta "Reservas hoy" → consume `fn_reservas_hoy`
- [ ] Tarjeta "% ocupacion hoy" → consume `fn_recursos_disponibles` y `fn_recursos_reservados_hoy`; el calculo del porcentaje se hace en C#:
  ```
  % = reservados_hoy / disponibles * 100
  ```

## Interpretacion de la metrica "% ocupacion hoy"

Representa que porcentaje del inventario bookable (recursos en estado `Disponible`) tiene al menos una reserva para el dia de hoy. No mide uso en tiempo real, mide compromiso del dia. Es una metrica estable que sirve para planificacion operativa.

## Criterio de pase

- [ ] Las 5 funciones devuelven valores consistentes con los datos de prueba
- [ ] Llamar cada funcion con `SELECT fn_nombre()` desde Workbench y verificar el resultado
- [ ] Insertar/cancelar reservas de prueba y verificar que los recuentos cambian coherentemente
- [ ] Verificar que `fn_recursos_disponibles` y `fn_recursos_reservados_hoy` son consistentes: la segunda no puede ser mayor que la primera

## Archivos a crear

- `Scripts SQL/Funciones.sql` — las 5 UDFs. Se ejecuta despues de `Coworking_db.sql` (necesita las tablas creadas).
