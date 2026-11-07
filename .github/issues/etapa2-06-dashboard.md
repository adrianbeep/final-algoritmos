# [Etapa 2] Dashboard con tarjetas KPI

## Objetivo

Implementar el panel del dashboard que se muestra cuando se hace click en "Dashboard" del sidebar. 4 tarjetas con metricas operativas alimentadas por las 5 UDFs del proyecto. Por ahora con datos hardcodeados, en la Etapa 3 se conectan a las UDFs reales.

## Entregables

### Layout

- [ ] TableLayoutPanel o estructura equivalente con 4 cards del mismo tamaño
- [ ] Distribucion: 2x2 o 1x4 segun el espacio disponible
- [ ] Cada card con:
  - Titulo arriba (label chico, color secundario)
  - Numero grande (label prominente, color principal o acento)
  - Label chico descriptivo abajo

### Las 4 tarjetas

- [ ] **Total clientes** → consume `fn_total_clientes`
- [ ] **Reservas por finalizar** → consume `fn_reservas_por_finalizar` (reservas en curso ahora mismo)
- [ ] **Reservas hoy** → consume `fn_reservas_hoy`
- [ ] **% ocupacion hoy** → consume `fn_recursos_disponibles` y `fn_recursos_reservados_hoy`, calculo del porcentaje en C#

### Calculo del porcentaje de ocupacion (en C#)

```
int disponibles = ObtenerRecursosDisponibles();
int reservadosHoy = ObtenerRecursosReservadosHoy();
double ocupacion = disponibles > 0 ? (reservadosHoy * 100.0 / disponibles) : 0;
lblOcupacion.Text = $"% Ocupacion hoy: {ocupacion:F1}%";
```

### Estilo

- [ ] Misma paleta de colores que el resto del proyecto
- [ ] Color de acento por tarjeta:
  - Total clientes: azul
  - Reservas por finalizar: amarillo/naranja
  - Reservas hoy: verde
  - % ocupacion: rojo si >80%, amarillo si >50%, verde si <50% (opcional)
- [ ] Tipografia jerarquizada: el numero grande usa font-size 28-32, el titulo 12-14
- [ ] Padding consistente (20px) dentro de cada card

## Criterio de pase

- [ ] Al hacer click en Dashboard en el sidebar, se muestran las 4 tarjetas
- [ ] Cada tarjeta muestra un numero (por ahora hardcodeado)
- [ ] El estilo es consistente con el sidebar y los otros forms
- [ ] El calculo del porcentaje funciona correctamente (probado con valores de prueba)

## Archivo a crear

- `SistemaCoworking/Forms/DashboardControl.cs` (UserControl, no Form)

## Por que UserControl y no Form

El dashboard se carga DENTRO del panel central del FormPrincipal, no se abre como ventana separada. Por eso es un UserControl, que es un "pedazo de UI" que se puede embeber dentro de otro form.

## Nota

- Por ahora los datos son hardcodeados (ej: "Total clientes: 5")
- En la Etapa 3 cada label se conecta a la UDF correspondiente via el DAO
- En la Etapa 6 se puede agregar un grafico con LiveCharts2 si da el tiempo (opcional, no es requisito)
