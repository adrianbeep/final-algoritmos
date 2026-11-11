# [Etapa 2] Se elimina el helper de los CheckBox

## Objetivo

`Herramientas/CheckBoxConfiguration.cs` dibuja a mano los checkbox con estilo
Fluent: una caja de 18 px con esquinas redondeadas y la tilde dibujada con
`DrawLines`, o un interruptor deslizante. Son 332 lineas y un unico caller:

```
RecursoModal.cs:84:   CheckBoxConfiguration.Config(chkProyector);
```

Es el unico helper de `Herramientas/` que se usa en un solo punto, y el mas
grande con diferencia. La consigna pide que el equipo pueda explicar todo lo que
entrega, y esto pide explicar un `ConditionalWeakTable`, una reflexion para
poner `DoubleBuffered` y dos `DrawLines` para la tilde, todo para un solo
checkbox.

Ademas, la mitad del helper estaba sin usar: `ConfigSwitch`, el interruptor
deslizante, no lo llamaba nadie.

## Entregables

- [ ] Se borra `Herramientas/CheckBoxConfiguration.cs`
- [ ] Se borra la llamada en `RecursoModal`
- [ ] El checkbox "Proyector" sigue funcionando, con el control nativo de
      Windows, con el color de las etiquetas de la app
- [ ] Se actualiza la referencia al helper en el issue 2.07

## Criterio de pase

- [ ] El modal de recurso abre y el checkbox "Proyector" se ve y se puede marcar
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Cambiar el checkbox por otro control (un toggle, un combo): el tipo del
      recurso ya se elige en el combo de arriba

## Nota

El control nativo con `FlatStyle.Flat` y el color de las etiquetas de la app
se ve igual que el resto de la interfaz: la tipografia ya era la misma
(Segoe UI 9), asi que solo hacen falta el `FlatStyle` y el `ForeColor`.
